import React from "react";
import { RateLimiter } from "@/utils/rateLimiter";
import { setStateAsyncFactory } from "@utils/stateSetter";
import { RenderedCanvas } from "@/models/renderedCanvas";
import { calculateDefaultPanZoomSettings, getDefaultPanZoomSettings, PanZoomSettings, translatePanZoomSettingsForNewSize } from "@/models/panZoomSettings";
import { CanvasViewerSettings } from "@/models/canvasViewerSettings";
import { RenderedCanvasPainter } from "@/engine/renderedCanvasPainter";
import "./canvasEditor.scss";
import { PxPoint } from "@/models/pxPoint";
import { ProgressBarGameStyle } from "../progressBarGameStyle";
import { ProgressX } from "@/utils/progressX";
import { Logger } from "@/utils/logger";

interface CanvasEditorProps {
    zLayerFilter?: number;
    isSolidColors: boolean;
}

interface CanvasEditorState {
    isLoading: boolean;
    isResizing: boolean;
}

export class CanvasEditor extends React.PureComponent<
    CanvasEditorProps,
    CanvasEditorState
> {
    private refCanvas: React.RefObject<HTMLCanvasElement | null> = React.createRef<HTMLCanvasElement>();
    private resizeLimiter: RateLimiter = new RateLimiter(100);
    private setStateAsync = setStateAsyncFactory(this);
    private paintTimerTask?: number;
    private painter?: RenderedCanvasPainter;
    private panZoomSettings: PanZoomSettings;
    private renderedCanvas?: RenderedCanvas;

    constructor(props: CanvasEditorProps) {
        super(props);
        this.onResize = this.onResize.bind(this);
        this.onResizeActual = this.onResizeActual.bind(this);
        this.onPainterTick = this.onPainterTick.bind(this);
        this.doRepaint = this.doRepaint.bind(this);
        this.panZoomSettings = { /* screw it.*/ } as PanZoomSettings;
        this.state = { isLoading: true, isResizing: false };
    }

    public static defaultProps: Partial<CanvasEditorProps> = {
        isSolidColors: true
    };

    public async setCanvasAsync(rc: RenderedCanvas) {
        if (this.refCanvas.current) {
            await this.setStateAsync({ isLoading: true });
            const { width, height } = rc;
            const canvas = this.refCanvas.current;
            const viewerWidth = canvas.width;
            const viewerHeight = canvas.height;
            this.panZoomSettings = calculateDefaultPanZoomSettings(width, height, viewerWidth, viewerHeight);
            this.restrictZoom();
            await ProgressX.ReportAsync(10, "Preparing to render everything...");
            this.renderedCanvas = rc;
            this.painter = await RenderedCanvasPainter.CreateAsync(
                rc!,
                10
            );
            await this.setStateAsync({ isLoading: false });
        }
    }

    // ------  doInitialize()  --------------------------------------------------------------------
    // Initialize the nav menu and set up all ARIA states, and add any missing classes as needed.
    // Adds aria-expanded, aria-hidden, aria-haspopup, .f-nav-wrapper... adds helper classes to
    // stylize links and buttons.  Also adds the c-navbar-fixed-spacer if the c-navbar has the
    // f-fixed modifier class.
    public async componentDidMount() {
        window.addEventListener("resize", this.onResize);
        window.addEventListener('mousedown', this.onMouseDown);
        window.addEventListener('mouseup', this.onMouseUp);
        window.addEventListener('mousemove', this.onMouseMove);
        window.addEventListener('wheel', this.onScroll, { passive: false });

        window.addEventListener('touchstart', this.onTouchStart);
        window.addEventListener('touchend', this.onTouchEnd);
        window.addEventListener('touchmove', this.onTouchMove);
        this.onResize();

        this.paintTimerTask = window.setInterval(this.onPainterTick, 10);
    }

    public componentWillUnmount() {
        window.removeEventListener("resize", this.onResize);
        window.removeEventListener('mousedown', this.onMouseDown);
        window.removeEventListener('mouseup', this.onMouseUp);
        window.removeEventListener('mousemove', this.onMouseMove);
        window.removeEventListener('wheel', this.onScroll);
        window.removeEventListener('touchstart', this.onTouchStart);
        window.removeEventListener('touchend', this.onTouchEnd);
        window.removeEventListener('touchmove', this.onTouchMove);

        this.resizeLimiter.cancel();
        // document.getElementsByTagName("body")[0].style.overflowY = "auto";
        if (this.paintTimerTask != undefined)
            window.clearInterval(this.paintTimerTask);
    }

    public async componentDidUpdate(
        prevProps: CanvasEditorProps,
        prevState: CanvasEditorState
    ) {
    }


    // ------  touchListeners()  --------------------------------------------------------------------
    private initialTouchPoints: Touch[] = [];
    private isTouching: boolean = false;
    private initialZoom: number = 0;

    private onTouchStart = (e: TouchEvent) => {
        this.isTouching = true;
        this.initialTouchPoints = [];
        this.initialZoom = this.panZoomSettings.zoomLevel;
        this.panZoomSettings.initialImageX = this.panZoomSettings.imageX;
        this.panZoomSettings.initialImageY = this.panZoomSettings.imageY;

        for (let i = 0; i < e.touches.length; i++) {
            const touch = e.touches.item(i);
            if (!!touch) {
                this.initialTouchPoints.push(touch);
            }
        }
    }

    private onTouchEnd = (e: TouchEvent) => {
        this.isTouching = false;
        this.initialTouchPoints = [];
    }

    private onTouchMove = (e: TouchEvent) => {
        const pz = this.panZoomSettings;
        if (this.isTouching) {
            if (e.touches.length === 1) {
                const point: PxPoint = this.getPointOnCanvasFromTouchEvent(e.touches.item(0)!);
                const initialTouchPoint = this.getPointOnCanvasFromTouchEvent(this.initialTouchPoints[0]);
                pz.imageX = pz.initialImageX - (initialTouchPoint.x - point.x);
                pz.imageY = pz.initialImageY - (initialTouchPoint.y - point.y);
            } else if (e.touches.length === 2) {
                const touches = [];
                for (let i = 0; i < e.touches.length; i++) {
                    const touchNow = e.touches.item(i);
                    const touchBefore = this.initialTouchPoints.find(t => t.identifier == touchNow?.identifier);
                    if (!touchNow || !touchBefore) continue;
                    const pntBefore = this.getPointOnCanvasFromTouchEvent(touchBefore);
                    const pntNow = this.getPointOnCanvasFromTouchEvent(touchNow);
                    touches.push({ touchNow, touchBefore, pntNow, pntBefore });
                }

                const avgMultiplier = (1 / touches.length);
                const avgInitialPoint = touches.reduce<PxPoint>((cum, cur) => {
                    cum.x += cur.pntBefore.x * avgMultiplier;
                    cum.y += cur.pntBefore.y * avgMultiplier;
                    return cum;
                }, { x: 0, y: 0 } as PxPoint);

                const avgFinalPoint = touches.reduce<PxPoint>((cum, cur) => {
                    cum.x += cur.pntNow.x * avgMultiplier;
                    cum.y += cur.pntNow.y * avgMultiplier;
                    return cum;
                }, { x: 0, y: 0 } as PxPoint);

                const distanceFromAvgPointBefore = touches.reduce<number>((cum, cur) => {
                    cum += Math.sqrt(Math.pow((avgInitialPoint.x - cur.pntBefore.x), 2)
                        + Math.pow((avgInitialPoint.y - cur.pntBefore.y), 2))
                        * avgMultiplier;
                    return cum;
                }, 0);

                const distanceFromAvgPointNow = touches.reduce<number>((cum, cur) => {
                    cum += Math.sqrt(Math.pow((avgFinalPoint.x - cur.pntNow.x), 2)
                        + Math.pow((avgFinalPoint.y - cur.pntNow.y), 2))
                        * avgMultiplier;
                    return cum;
                }, 0);

                // Logic for moving canvas with two fingers
                {
                    // // Change zoom based on button presses
                    // pz.zoomLevel = this.initialZoom * (distanceFromAvgPointNow / distanceFromAvgPointBefore);
                    // this.restrictZoom();

                    // // move to where fingers are going.
                    // pz.imageX = pz.initialImageX - (avgInitialPoint.x - avgFinalPoint.x);
                    // pz.imageY = pz.initialImageY - (avgInitialPoint.y - avgFinalPoint.y);

                }

                // Zoom with two fingers 
                {
                    // // fetches point on canvas, relative to top left edge of canvas
                    // // fetches point on image, relative to src img.
                    // const imagePoint: PxPoint = RenderedCanvasPainter.GetPointOnImage(avgFinalPoint, pz, 'round');

                    // pz.zoomLevel = this.initialZoom * (distanceFromAvgPointNow / distanceFromAvgPointBefore);
                    // this.restrictZoom();
                    // pz.imageX = Math.round(avgFinalPoint.x - (imagePoint.x * pz.zoomLevel));
                    // pz.imageY = Math.round(avgFinalPoint.y - (imagePoint.y * pz.zoomLevel));
                }

                // TODO: Merge the above two blocks so that both benefits are gained. Do it in this block below somehow.
                {
                    // Zoom with two fingers 
                    {
                        // fetches point on image, relative to src img.
                        const imagePoint = {
                            x: Math.round((avgFinalPoint.x - pz.imageX) / pz.zoomLevel),
                            y: Math.round((avgFinalPoint.y - pz.imageY) / pz.zoomLevel)
                        }

                        pz.zoomLevel = this.initialZoom * (distanceFromAvgPointNow / distanceFromAvgPointBefore);
                        this.restrictZoom();

                        // do zoom stuff
                        pz.imageX = Math.round(avgFinalPoint.x - (imagePoint.x * pz.zoomLevel));
                        pz.imageY = Math.round(avgFinalPoint.y - (imagePoint.y * pz.zoomLevel));

                        // // move to where fingers are going.
                        // pz.imageX = pz.initialImageX - (avgInitialPoint.x - avgFinalPoint.x);
                        // pz.imageY = pz.initialImageY - (avgInitialPoint.y - avgFinalPoint.y);

                    }
                }
            }
        }
    }

    private getPointOnCanvasFromTouchEvent = (e: Touch) => {
        if (!!this.refCanvas.current) {
            var rect = this.refCanvas.current.getBoundingClientRect();
            var x = e.clientX - rect.left; //x position within the element.
            var y = e.clientY - rect.top;  //y position within the element.
            return { x, y } as PxPoint;
        }

        return { x: 0, y: 0 } as PxPoint;
    }

    // ------  mouseListeners()  --------------------------------------------------------------------
    private initialDragPoint: PxPoint = { x: 0, y: 0 };
    private isDragging: boolean = false;
    private onMouseMove = (e: MouseEvent) => {
        if (this.isDragging) {
            const point = this.getPointOnCanvasFromMouseEvent(e);
            this.panZoomSettings.imageX = this.panZoomSettings.initialImageX - (this.initialDragPoint.x - point.x);
            this.panZoomSettings.imageY = this.panZoomSettings.initialImageY - (this.initialDragPoint.y - point.y);
        }
    }

    private onMouseUp = (e: MouseEvent) => {
        this.isDragging = false;
    }

    private onMouseDown = (e: MouseEvent) => {
        this.isDragging = true;
        const panelPoint: PxPoint = this.getPointOnCanvasFromMouseEvent(e);
        this.initialDragPoint = panelPoint;
        this.panZoomSettings.initialImageX = this.panZoomSettings.imageX;
        this.panZoomSettings.initialImageY = this.panZoomSettings.imageY;
    }

    // fetches point on canvas, relative to top left edge of canvas
    private getPointOnCanvasFromMouseEvent = (e: MouseEvent) => {
        if (!!this.refCanvas.current) {
            var rect = this.refCanvas.current.getBoundingClientRect();
            var x = e.clientX - rect.left; //x position within the element.
            var y = e.clientY - rect.top;  //y position within the element.
            return { x, y } as PxPoint;
        }

        return { x: 0, y: 0 } as PxPoint;
    }


    private onScroll = (e: WheelEvent) => {
        e.preventDefault();
        if (e.deltaY != 0) {
            // fetches point on canvas, relative to top left edge of canvas
            const panelPoint: PxPoint = this.getPointOnCanvasFromMouseEvent(e);
            // fetches point on image, relative to src img.
            const imagePoint: PxPoint = RenderedCanvasPainter.GetPointOnImage(panelPoint, this.panZoomSettings, 'round');

            if (e.deltaY > 0) {
                this.panZoomSettings.zoomLevel *= 0.80;
            }
            else {
                this.panZoomSettings.zoomLevel *= 1.25;
            }

            this.restrictZoom();
            this.panZoomSettings.imageX = Math.round(panelPoint.x - (imagePoint.x * this.panZoomSettings.zoomLevel));
            this.panZoomSettings.imageY = Math.round(panelPoint.y - (imagePoint.y * this.panZoomSettings.zoomLevel));
        }
    }


    // -------  onPaint()  -----------------------------------------------------------------------
    private onPainterTick(): void {
        // if (!this.isRepaintRequested) return;
        if (!this.refCanvas.current) return;
        this.doRepaint();
    }

    private async doRepaint(): Promise<void> {
        if (!this.refCanvas.current) return;
        const canvas = this.refCanvas.current;
        const ctx = canvas.getContext("2d")!;
        ctx.imageSmoothingEnabled = false;
        if (!this.painter) return;
        this.painter.PaintSurface(canvas, this.panZoomSettings, {
            IsShowBorder: false,
            IsSolidColors: false,
        });

    }


    // -------  onResize()  -----------------------------------------------------------------------
    // Handles setting of IsMobile, handles any changes between mobile and desktop that are needed.
    private onResize = () => {
        this.setStateAsync({ isResizing: true });
        this.resizeLimiter.tryAction(this.onResizeActual);
    }

    private async onResizeActual(): Promise<void> {
        if (!this.refCanvas.current) return;
        const canvas = this.refCanvas.current;
        // Make it visually fill the positioned parent
        // canvas.style.width ='100%';
        // canvas.style.height='100%';
        // // ...then set the internal size to match

        const pe = canvas.parentElement!;
        canvas.width = pe.offsetWidth;
        canvas.height = pe.offsetHeight;

        // Every time you resize, the height grows by 4. Why?
        // Logger.log('on-resize' + `${pe.clientHeight}, ${pe.offsetHeight}, ${pe.scrollHeight}`);

        if (!!this.renderedCanvas) {
            const { width, height } = this.renderedCanvas;
            const canvas = this.refCanvas.current;
            const viewerWidth = canvas.width;
            const viewerHeight = canvas.height;

            //            this.panZoomSettings = calculateDefaultPanZoomSettings(width, height, viewerWidth, viewerHeight);
            this.panZoomSettings = translatePanZoomSettingsForNewSize(this.panZoomSettings, width, height, viewerWidth, viewerHeight);
            // this.painter = await RenderedCanvasPainter.CreateAsync(
            //     this.props.renderedCanvas!,
            //     1
            // );
            this.restrictZoom();
        }

        this.setStateAsync({ isResizing: false });
        await this.doRepaint();
    }

    private restrictZoom = () => {
        this.panZoomSettings.zoomLevel
            = (this.panZoomSettings.zoomLevel < this.panZoomSettings.minZoomLevel
                ? this.panZoomSettings.minZoomLevel
                : this.panZoomSettings.zoomLevel > this.panZoomSettings.maxZoomLevel
                    ? this.panZoomSettings.maxZoomLevel
                    : this.panZoomSettings.zoomLevel);
    }


    public render() {
        let classNms = 'canvasEditor';
        if (this.state.isLoading) classNms += ' f-loading';
        if (this.state.isResizing) classNms += ' f-resizing';

        return (
            <div className={classNms}>
                {this.state.isLoading && <div className='loadingPanel'>
                    <div>
                        <img src='./assets/img/txt_paused.png' alt='paused' />
                        <ProgressBarGameStyle />
                    </div>
                </div>}
                <div className='cc-canvasContainer'>
                    <canvas ref={this.refCanvas} id='ctxTest'></canvas>
                </div>
            </div>
        );
    }
}