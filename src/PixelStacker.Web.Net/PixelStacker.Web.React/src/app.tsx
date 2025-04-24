import React from 'react';
import { CanvasEditor, /* loadFile */ } from './components/canvasEditor/canvasEditor';
import { MaterialPalette } from './models/materialPalette';
import { RenderedCanvas } from './models/renderedCanvas';
import './app.scss'
import { ProgressX } from './utils/progressX';
import { NavMenu } from './components/navMenu';
import { FileUploader } from './components/fileUploader';
import { Logger } from './utils/logger';
import { Constants } from './constants';

interface AppProps { }
interface AppState {
    renderedCanvas?: RenderedCanvas;
}

export class App extends React.PureComponent<AppProps, AppState> {
    private refEditor: React.RefObject<CanvasEditor | null> = React.createRef<CanvasEditor>();
    private refFileUploader: React.RefObject<FileUploader | null> = React.createRef<FileUploader>();

    constructor(props: AppProps) {
        super(props);
        (window as any)['progressX'] = ProgressX;
        this.state = {
            renderedCanvas: {
                isSideView: false,
                height: 2,
                width: 2,
                canvasData: [[39, 1], [39, 39]],
                palette: MaterialPalette.fromResx()
            } as RenderedCanvas
        }
    }


    // I just tried copying some code from MDN but i did a bit of googling and you
    // might need to handle drag-n-drop a different way in react. either way
    // im gonnna go have some lunch rn
    // 
    // private onDragOver(e: DragEvent<HTMLDivElement>) {
    //   // Some indicator of "you can drop this file"
    //   Logger.log(e);
    // }

    private onDrop(e: DragEvent) {
        e.preventDefault();
        if (!e.dataTransfer) return;
        if (e.dataTransfer.items) {
            // Use DataTransferItemList interface to access the file(s)
            for (let i = 0; i < e.dataTransfer.items.length; i++) {
                // If dropped items aren't files, reject them
                if (e.dataTransfer.items[i].kind === 'file') {
                    const file = e.dataTransfer.items[i].getAsFile();
                    Logger.log(`… file[${i}].name = ${file!.name}`);
                }
            }
        } else {
            // Use DataTransfer interface to access the file(s)
            for (let i = 0; i < e.dataTransfer.files.length; i++) {
                Logger.log(`… file[${i}].name = ${e.dataTransfer?.files[i].name}`);
            }
        }
    }

    public render(): React.ReactNode {
        return (
            <div className="App"
            // onDragOver={this.onDragOver}
            >
                <div className='pageLayout'>
                    <NavMenu
                        topNavTitle='Main'
                        title={`PixelStacker Web`}
                        colorTheme='f-neutral'
                        isFixed={false}
                        defaultItems={[{
                            text: 'Open', shortcutKeys: 'CTRL + O', onClick: async () => {
                                this.refFileUploader.current?.doPickFile();
                            }
                        },
                            // {
                            //   text: 'File', items: [
                            //     {
                            //       text: 'Open', shortcutKeys: 'CTRL + O', onClick: async () => {
                            //         this.refFileUploader.current?.doPickFile();
                            //       }
                            //     },
                            //     // {text: 'Save', shortcutKeys: 'CTRL + S', onClick: async () => { } },
                            //     // {text: 'Export Settings', onClick: async () => { } },
                            //   ]
                            // },
                            // {
                            //   text: 'Edit', items: [
                            //     { text: 'Undo', shortcutKeys: 'CTRL + Z', onClick: async () => Logger.log('click') },
                            //     { text: 'Redo', shortcutKeys: 'CTRL + Y', onClick: async () => Logger.log('click') }
                            //   ]
                            // },
                            // {
                            //   text: 'View', items: [
                            //     { text: 'Toggle Grid', onClick: async () => Logger.log('Click') },
                            //     { text: 'Toggle Texture', onClick: async () => Logger.log('Click') },
                            //     { text: 'Toggle Border', onClick: async () => Logger.log('Click') },
                            //     {
                            //       text: 'Layer filtering', items: [
                            //         { text: 'Show all layers', onClick: async () => Logger.log('Click') },
                            //         { text: 'Show bottom layer', onClick: async () => Logger.log('Click') },
                            //         { text: 'Show top layer', onClick: async () => Logger.log('Click') }
                            //       ]
                            //     }
                            //   ]
                            // },
                            // {
                            //   text: 'Tools', items: [
                            //     // { text: 'Material Options', shortcutKeys: 'CTRL + E', onClick: async () => Logger.log('click') },
                            //     // { text: 'Quantizer Options', shortcutKeys: 'CTRL + Q', onClick: async () => Logger.log('click') },
                            //     // { text: 'Other Options', shortcutKeys: 'CTRL + SHIFT + E', onClick: async () => Logger.log('click') },
                            //     { text: 'Render', shortcutKeys: 'CTRL + R', onClick: async () => Logger.log('click') }
                            //   ]
                            // },
                            // { text: 'Language', onClick: async () => Logger.log('click') },
                            // {
                            //   text: 'Help', items: [
                            //     // { text: 'About', onClick: async () => Logger.log('click') },
                            //     { text: 'Reload', onClick: async () => window.location.reload() },
                            //   ]
                            // }
                        ]}
                    />
                    <div className='pageLayoutContent'>
                        {/* <div
              style={{
                position: "absolute",
                top: 0,
                right: 0,
                left: 0,
                height: "32px",
                padding: "16px",
              }}
            >
              <TopBar
                onLoadFileStart={() => {
                  if (!!this.refEditor.current)
                    this.refEditor.current.setState({ isLoading: true });
                }}
                onLoadFileFinish={(d: number[][]) => this.topBarCallback(d)}
              />
            </div> */}
                        <CanvasEditor
                            ref={this.refEditor}
                        />
                        <FileUploader
                            ref={this.refFileUploader}
                            MaxHeight={Constants.MAX_HEIGHT}
                            MaxWidth={Constants.MAX_WIDTH}
                            onLoadFileStart={() => {
                                if (!!this.refEditor.current)
                                    this.refEditor.current.setState({ isLoading: true });
                            }}
                            onLoadFileError={(msg) => {
                                if (!!this.refEditor.current)
                                    this.refEditor.current.setState({ isLoading: false });
                                alert(msg);
                            }}
                            onLoadFileFinish={(isSuccess, design) => {
                                if (this.refEditor.current)
                                    if (isSuccess && !!design)
                                        this.refEditor.current.setCanvasAsync(design);
                            }}
                        />
                    </div>
                </div>
            </div>
        );
    }
}

export default App;
