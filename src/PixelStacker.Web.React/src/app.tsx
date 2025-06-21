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
  maxWidth: number; // Purely used for updating the state.
  maxHeight: number;
}

export class App extends React.PureComponent<AppProps, AppState> {
  private refEditor: React.RefObject<CanvasEditor | null> = React.createRef<CanvasEditor>();
  private refFileUploader: React.RefObject<FileUploader | null> = React.createRef<FileUploader>();

  constructor(props: AppProps) {
    super(props);
    (window as any)['progressX'] = ProgressX;
    this.state = {
      renderedCanvas: undefined,
      maxWidth: Constants.MAX_WIDTH,
      maxHeight: Constants.MAX_HEIGHT
    }
  }

  async componentDidMount(): Promise<void> {
    const palette = await MaterialPalette.fromApi();
    this.setState({
      renderedCanvas: {
        isSideView: false,
        height: 2,
        width: 2,
        canvasData: [[39, 1], [39, 39]],
        palette: palette
      } as RenderedCanvas
    })
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
            }, {
              text: 'Click for tips',
              items: [
                {
                  text: "Scroll to zoom in and out.", onClick: async () => { }
                },
                {
                  text: `Current size cap: ${this.state.maxWidth}x${this.state.maxHeight}`, onClick: async () => { 
                    const numbr = window.prompt("Should a new max size be used? Provide a number between 16 and 4000 for the new max height and max width.");
                    if (numbr) {
                      let parsed = parseInt(numbr, 10);
                      if (!isNaN(parsed)) {
                        parsed = Math.max(4, Math.min(4000, parsed));
                        await new Promise<void>((resolve) => { 
                          this.setState({
                            maxHeight: parsed,
                            maxWidth: parsed
                          }, () => resolve());
                        });
                      }
                    }
                  }
                }
              ]
            }, {
              text: "Links",
              items: [
                { text: "Try the web API", to: "https://taylorlove.info/projects/pixelstacker/swagger/index.html" },
                { text: "Features", to: "https://taylorlove.info/pixelstacker/" },
                { text: "Source code", to: "https://github.com/Pangamma/PixelStacker" },
                {
                  text: "Downloads", items: [
                    { text: "Desktop version", to: "https://www.spigotmc.org/resources/pixelstacker-photo-realistic-pixel-art-generator.46812/" },
                    { text: "Spigot version", to: "https://www.spigotmc.org/resources/pixel-art-generator.125439/" }
                  ]
                }
              ]
            }
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
              MaxHeight={this.state.maxHeight}
              MaxWidth={this.state.maxWidth}
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
