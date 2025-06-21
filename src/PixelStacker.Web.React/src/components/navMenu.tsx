import { RateLimiter } from '@/utils/rateLimiter';
import { forceUpdateAsyncFactory, setStateAsyncFactory } from '@/utils/stateSetter';
import * as React from 'react';
import './navMenu.scss';
// import { Link } from 'react-router-dom';

interface TreeNodeMetaData {
    text: string;
    to?: string;
    shortcutKeys?: string;
    onClick?: () => Promise<void>;
    isExpanded: boolean;
}

/**
 * 
 * TODO: Look into "height('') and .f-flip changes"
 * TODO: Tabindexes are wacky right now.
 * TODO: Whatever was going on with component.openElement (.f-flip)
 */
export type NavMenuItemProps = {
    text: string;
    to?: string;
    shortcutKeys?: string;
    onClick?: () => Promise<void>;
    items?: NavMenuItemProps[];
}

export interface NavMenuProps {
    title: string;
    isFixed: boolean;
    topNavTitle: string;
    colorTheme: undefined | 'f-blue' | 'f-white' | 'f-black' | 'f-green' | 'f-neutral' | 'f-purple' | 'f-orange' | 'f-dark-blue';

    // If changed, the menu will re-render and lose state data.
    defaultItems: NavMenuItemProps[];
}

type NavMenuState = {

    /** Forgive me, I'm sorry. It was old code and I am just porting it over.
     *  Navbars mobile friendly issues are a witch with a capital B. */
    isMobile: boolean;
    isMobileExpanded: boolean;
    isMobileLeftArrowVisible: boolean;
    isMobileRightArrowVisible: boolean;
    mobileTitleText: string;

    /**
     * The unique id of the currently active parent node. 
     */
    currentNavUniqueId?: string;
    forcedHeightOfSubMenus?: number;
}


interface TreeNode<T> {
    uniqueId: string;
    depth: number;
    parent?: TreeNode<T>;
    next?: TreeNode<T>;
    prev?: TreeNode<T>;
    children: TreeNode<T>[];
    val: T;
}

export class NavMenu extends React.PureComponent<NavMenuProps, NavMenuState> {
    private static MOBILE_BREAKPOINT = 728;
    private resizeLimiter: RateLimiter = new RateLimiter(25);
    private pathMap: Record<string, TreeNode<TreeNodeMetaData>> = {};
    private root: TreeNode<TreeNodeMetaData>;
    private setStateAsync = setStateAsyncFactory(this);
    private forceUpdateAsync = forceUpdateAsyncFactory(this);

    constructor(props: NavMenuProps) {
        super(props);

        this.root = this.initTreeMapFromPropsHelper(
            { text: this.props.topNavTitle, items: this.props.defaultItems },
            0,
            this.pathMap,
            { count: 0 }
        );

        // We use this syntax to create a table of contents.
        this.initTreeMapFromProps = this.initTreeMapFromProps.bind(this);
        this.setMobileArrowsForCurrentSubmenu = this.setMobileArrowsForCurrentSubmenu.bind(this);
        this.doResizeSubMenuHeights = this.doResizeSubMenuHeights.bind(this);

        this.doCloseElementRecursive = this.doCloseElementRecursive.bind(this);
        this.doOpenElementRecursiveAndCloseSiblings = this.doOpenElementRecursiveAndCloseSiblings.bind(this);
        this.doOpenElementRecursive = this.doOpenElementRecursive.bind(this);

        this.onResize = this.onResize.bind(this);
        this.onResizeActual = this.onResizeActual.bind(this);
        this.onClickToggleMobileMenu = this.onClickToggleMobileMenu.bind(this);
        this.onClickMobilePrevButton = this.onClickMobilePrevButton.bind(this);
        this.onClickSubMenuButton = this.onClickSubMenuButton.bind(this);

        this.onRenderNavMenuItem = this.onRenderNavMenuItem.bind(this);

        this.state = {
            isMobile: false,
            mobileTitleText: props.title,
            isMobileExpanded: false,
            isMobileLeftArrowVisible: false,
            isMobileRightArrowVisible: false
        };
    }

    static defaultProps: NavMenuProps = {
        title: 'M&T Coffee',
        defaultItems: [],
        colorTheme: 'f-white',
        isFixed: false,
        topNavTitle: 'Main Menu'
    }

    private initTreeMapFromPropsHelper(
        oParent: NavMenuItemProps,
        depth: number,
        pathMapRef: Record<string, TreeNode<TreeNodeMetaData>>,
        counterRef: { count: number }
    ): TreeNode<TreeNodeMetaData> {
        const nParent: TreeNode<TreeNodeMetaData> = {
            uniqueId: `nav-node-${counterRef.count++}`,
            depth,
            val: {
                text: oParent.text,
                to: oParent.to,
                onClick: oParent.onClick,
                shortcutKeys: oParent.shortcutKeys,
                isExpanded: false
            },
            children: (oParent.items || []).map(oChild => this.initTreeMapFromPropsHelper(oChild, depth + 1, pathMapRef, counterRef))
        };

        pathMapRef[nParent.uniqueId] = nParent;

        for (let i = 0; i < nParent.children.length; i++) {
            nParent.children[i].parent = nParent;
            nParent.children[i].next = nParent.children[i + 1]; // undefined if not found.
            nParent.children[i].prev = nParent.children[i - 1]; // undefined if not found.
        }

        return nParent;
    }

    private initTreeMapFromProps() {
        const pathMap: Record<string, TreeNode<TreeNodeMetaData>> = {};
        const root = this.initTreeMapFromPropsHelper(
            { text: this.props.topNavTitle, items: this.props.defaultItems },
            0,
            pathMap,
            { count: 0 }
        );

        this.root = root;
        this.pathMap = pathMap;
    }

    // ------  doInitialize()  --------------------------------------------------------------------
    // Initialize the nav menu and set up all ARIA states, and add any missing classes as needed.
    // Adds aria-expanded, aria-hidden, aria-haspopup, .f-nav-wrapper... adds helper classes to 
    // stylize links and buttons.  Also adds the c-navbar-fixed-spacer if the c-navbar has the
    // f-fixed modifier class.
    public componentDidMount() {
        window.addEventListener('resize', this.onResize);
        this.onResize();
        this.initFocus();
    }

    public componentWillUnmount() {
        window.removeEventListener('resize', this.onResize);
        this.resizeLimiter.cancel();
        document.getElementsByTagName('body')[0].style.overflowY = 'auto';
    }

    public componentDidUpdate(prevProps: Readonly<NavMenuProps>, prevState: Readonly<NavMenuState>, snapshot?: any): void {
        if (prevProps.defaultItems !== this.props.defaultItems) {
            this.initTreeMapFromProps();
            this.setState({
                isMobile: false,
                mobileTitleText: this.props.title,
                isMobileExpanded: false,
                isMobileLeftArrowVisible: false,
                isMobileRightArrowVisible: false
            });
        }
    }

    // -------  Focus Listeners  ----------------------------------------------------------------------
    // For when regular focus checkers fail, and you need to track it.
    private prevFocus: Element | undefined = undefined;
    private curFocus: Element | undefined = undefined;
    private initFocus = () => {

        window.addEventListener('focusin', (ev: FocusEvent) => {
            const ncur = document.activeElement ? document.querySelector(':focus') : document.activeElement;
            if (!!ncur) {
                this.prevFocus = this.curFocus;
                this.curFocus = ncur;
            }
        });

        window.addEventListener('blur', (ev: FocusEvent) => {
            const elem = ev.target as Element;
            if (!!elem && elem.classList) {
                elem.classList.remove('x-hidden-focus');
            }
        });
        // -------  Focus Visibility  ---------------------------------------------------------------------
        // Add/Remove classes to body to mark when keyboard navigation is in progress.
        let isFocusVisible = false;
        window.addEventListener('mousedown', (ev: MouseEvent) => {
            if (isFocusVisible){
                isFocusVisible = false;
                const body: HTMLBodyElement = document.getElementsByTagName('body')[0];
                body.classList.add('x-focus-hidden');
                body.classList.remove('x-focus-visible');
            }

            const elem = ev.target as Element;
            if (!!elem && elem.classList) {
                elem.classList.add('x-hidden-focus');
            }
        });

        window.addEventListener('keyup', (ev: KeyboardEvent) => {
            if (!isFocusVisible) {
                isFocusVisible = true;
                const body: HTMLBodyElement = document.getElementsByTagName('body')[0];
                body.classList.add('x-focus-visible');
                body.classList.remove('x-focus-hidden');

                document.querySelectorAll('.x-hidden-focus').forEach((elem) => elem.classList.remove('x-hidden-focus'));
            }
        });

        const body: HTMLBodyElement = document.getElementsByTagName('body')[0];
        body.classList.add('x-focus-hidden');
    }

    // ------  closeElementRecursive()  -----------------------------------------------------------
    // Close all opened submenus. Reset submenu UL heights. If a selector is provided, then only
    // the submenus below the element will be closed.
    private async doCloseElementRecursive(node: TreeNode<TreeNodeMetaData>): Promise<void> {
        node.val.isExpanded = false;
        for (let i = 0; i < node.children.length; i++) {
            await this.doCloseElementRecursive(node.children[i]);
        }
    }

    private getSiblingNodes(node: TreeNode<TreeNodeMetaData>): TreeNode<TreeNodeMetaData>[] {
        if (!node.parent) return [];
        return node.parent.children.filter(n => n.uniqueId !== node.uniqueId);
    }

    // ------  doOpenElementRecursiveAndCloseSiblings()  ---------------------------------------
    // Close all opened submenus EXCEPT for the submenus between the ROOT node and the passed in node.
    private async doOpenElementRecursiveAndCloseSiblings(node: TreeNode<TreeNodeMetaData>): Promise<void> {
        await this.doCloseElementRecursive(node); // close all children (and self)

        let cursor: TreeNode<TreeNodeMetaData> | undefined = node;
        while (cursor !== undefined) {
            cursor.val.isExpanded = true;
            this.getSiblingNodes(cursor).forEach(sib => this.doCloseElementRecursive(sib));
            cursor = cursor.parent;
        }
    }

    // ------  openElementRecursive()  ------------------------------------------------------------
    // Opens current element, adds f-current-nav to the newly opened submenu, and makes sure any 
    // parent menus are also open.
    private async doOpenElementRecursive(node: TreeNode<TreeNodeMetaData>): Promise<void> {
        await this.setStateAsync({
            currentNavUniqueId: node.uniqueId,
            mobileTitleText: node.val.text
        });

        do {
            node.val.isExpanded = true;
            if (node.parent === undefined) {
                break;
            } else {
                node = node.parent!;
            }
        } while (true);

        await this.setMobileArrowsForCurrentSubmenu();
    }

    // -----  setMobileArrowsForCurrentSubmenu()  -------------------------------------------------
    // Toggles visibility of the mobile f-right and f-left arrows.
    private async setMobileArrowsForCurrentSubmenu(): Promise<void> {
        if (!this.state.currentNavUniqueId) return;
        const curNav = this.pathMap[this.state.currentNavUniqueId!];

        await this.setStateAsync({
            isMobileLeftArrowVisible: curNav.depth > 0,
            isMobileRightArrowVisible: false
        });

    }

    // -------  onResize()  -----------------------------------------------------------------------
    // Handles setting of IsMobile, handles any changes between mobile and desktop that are needed.
    private onResize() {
        this.resizeLimiter.tryAction(this.onResizeActual);
    }

    private async onResizeActual(): Promise<void> {
        if (!this.state.isMobile) {
            if (window.innerWidth < NavMenu.MOBILE_BREAKPOINT) {
                await this.setStateAsync({ isMobile: true });
                await this.doCloseElementRecursive(this.root);
            }
        } else {
            if (window.innerWidth >= NavMenu.MOBILE_BREAKPOINT) {
                document.getElementsByTagName('body')[0].style.overflowY = 'auto'; // Re-enable scrolling just in case
                await this.setStateAsync({ isMobile: false });

                // Ensure proper ARIA rules
                await this.doOpenElementRecursiveAndCloseSiblings(this.root); // $('.c-navbar .f-nav-row > button').first()
            }
        }
    }


    //#region Buttons / Controls
    // ------  onClickSubMenuButton()  ------------------------------------------------------------
    // Click (Closed) --> Open submenu
    // if (Menu open will go off screen) then add "f-flip" class to the UL or something.
    // if mobile view... do other complex logic with back buttons and other things.
    // if (side by side with other submenu, set smaller so heights match bigger)
    private async onClickSubMenuButton(node: TreeNode<TreeNodeMetaData>): Promise<void> {
        if (!node.val.isExpanded) {
            await this.doOpenElementRecursiveAndCloseSiblings(this.root);
            await this.doOpenElementRecursive(node);
            if (this.state.isMobile) {
                const navElem = document.getElementById(node.uniqueId);
                const liElem = navElem && navElem.querySelector('ul > li');
                const btnElem = liElem && (liElem.classList.contains('f-nav-link') ? liElem.querySelector('a') : liElem.querySelector('button'));

                if (btnElem) {
                    await this.forceUpdateAsync(); // force render so button appears. Then focus.
                    (btnElem as (HTMLElement)).focus();
                }
            }
        } else {
            await this.doCloseElementRecursive(node);
            // $(element).closest('.f-nav-row').find('ul').height('');
        }
        await this.forceUpdateAsync();
        await this.doResizeSubMenuHeights();
        // if ($(element).is("li.f-nav-menu>button")) {
        //     $e = $(element);
        //     if ($e.attr('aria-expanded') === 'false') {
        //         // this.closeElementRecursive();
        //         // this.openElementRecursive(element);
        //         if (this.isMobile) {
        //             $(element).next('ul').children('li:first-child').children('a,button').addClass('x-hidden-focus').focus();
        //             component.setTabIndexForCurrentSubmenu();
        //         }
        //     } 
        //     // else if ($e.attr('aria-expanded') === 'true') {
        //     //     // Click (Opened) --> Close submenu and sub submenus.
        //     //     this.closeElementRecursive(element);
        //     //     $(element).closest('.f-nav-row').find('ul').height('');
        //     // }
        // }
    }

    // ------  onClickMobilePrevButton()  ---------------------------------------------------------
    // If sub menu, go UP one menu. If top level, try to go back one menu (There can be multiple
    // menu rows).
    public async onClickMobilePrevButton(): Promise<void> {
        if (!this.state.currentNavUniqueId) return;
        const curNav = this.pathMap[this.state.currentNavUniqueId!];
        const newUniqueId = (curNav && curNav.parent && curNav.parent.uniqueId) || undefined;
        await this.doCloseElementRecursive(curNav);

        if (!!curNav.parent) {
            await this.doOpenElementRecursive(curNav.parent!);
        }

        await this.setStateAsync(ps => ({ ...ps, currentNavUniqueId: newUniqueId }));
        this.setMobileArrowsForCurrentSubmenu();

        // set focus to upper element.
        const liElem = document.getElementById(curNav.uniqueId);
        const btnElem = liElem && liElem.firstElementChild;
        if (btnElem) {
            (btnElem as HTMLElement).focus();
        }
    }

    private async doResizeSubMenuHeights(): Promise<void> {
        if (!this.state.currentNavUniqueId) return;
        const curNav = this.pathMap[this.state.currentNavUniqueId!]!;
        if (curNav.depth === 1 && this.state.forcedHeightOfSubMenus !== undefined) {
            await this.setStateAsync({ forcedHeightOfSubMenus: undefined });
            document.querySelectorAll('.c-navbar .f-flip').forEach(c => c.classList.remove('f-flip'));
        } else if (curNav.depth === 2) {
            // we now have OPEN the 2 lowest navs allowed.
            const liElemChild = document.getElementById(curNav.uniqueId);
            const liElemParent = document.getElementById(curNav.parent!.uniqueId);

            if (liElemChild && liElemParent) {
                const ulChild = liElemChild.querySelector('ul');
                const ulParent = liElemParent.querySelector('ul');

                if (ulChild && ulParent) {

                    // if (Menu open will go off screen) then add "f-flip" class to the UL or something.
                    ulChild.style.height = '';
                    ulParent.style.height = '';
                    const childRect = ulChild.getBoundingClientRect()

                    const rightEdge = childRect.x + childRect.width;
                    const screenWidth = window.innerWidth;

                    if (rightEdge - screenWidth > 0) {
                        ulChild.classList.add('f-flip');
                    }

                    await this.setStateAsync({ forcedHeightOfSubMenus: Math.max(ulChild.offsetHeight, ulParent.offsetHeight) });
                }
            }
        }
    }

    // ------  onClickToggleMobileMenu()  --------------------------------------------------------------
    // Open mobile menu if closed. Close if opened. Activates top level nav as the currently active
    // navigation submenu.
    public async onClickToggleMobileMenu(): Promise<void> {
        if (this.state.isMobileExpanded) {
            document.getElementsByTagName('body')[0].style.overflowY = 'auto';
            await this.doCloseElementRecursive(this.root);
            await this.setStateAsync({
                currentNavUniqueId: undefined,
                isMobileExpanded: false,
            });
        } else {
            document.getElementsByTagName('body')[0].style.overflowY = 'hidden';
            await this.doOpenElementRecursive(this.root);
            await this.setMobileArrowsForCurrentSubmenu();
            await this.setStateAsync({
                currentNavUniqueId: this.root.uniqueId,
                isMobileExpanded: true,
            });
        }
    }


    //#endregion

    private onRenderNavMenuItem(itemProps: TreeNode<TreeNodeMetaData>, isCurrentNavChild: boolean): React.ReactNode {
        const { children, uniqueId, val } = itemProps;
        const { isExpanded, text, to, onClick, shortcutKeys } = val;

        const isSelectedNav = itemProps.uniqueId === this.state.currentNavUniqueId;
        const tabIndex = (!this.state.isMobile || isSelectedNav || isCurrentNavChild) ? 0 : -1;

        if (children.length > 0) {
            return (
                <li key={uniqueId} id={uniqueId}
                    className={`f-nav-menu f-nav-wrapper ${isSelectedNav ? 'f-current-nav' : ''}`}>
                    <button
                        tabIndex={tabIndex} aria-expanded={itemProps.val.isExpanded}
                        aria-haspopup={true}
                        onClick={() => this.onClickSubMenuButton(itemProps)}
                    >
                        {text} {shortcutKeys && `(${shortcutKeys})`}
                    </button>
                    <ul aria-hidden={isExpanded !== true} style={{ height: this.state.forcedHeightOfSubMenus }}>
                        {children.map((child) => this.onRenderNavMenuItem(child, itemProps.uniqueId === this.state.currentNavUniqueId))}
                    </ul>
                </li>
            );
        } else if (!!to) {
            return (<li className='f-nav-link' key={uniqueId}>
                <a href={to} tabIndex={tabIndex}
                onClick={async () => {
                    await this.doCloseElementRecursive(this.root);
                    await this.setStateAsync({ isMobileExpanded: false });
                    await this.forceUpdateAsync();
                }}>{text}</a>
            </li>);
        } else if (!!onClick) {
            return (<li className='f-nav-link' key={uniqueId}>
                <button tabIndex={tabIndex} onClick={async () => {
                    document.body.focus();
                    await this.doCloseElementRecursive(this.root);
                    await this.setStateAsync({ isMobileExpanded: false });
                    await this.forceUpdateAsync();
                    await onClick();
                }}>{text}</button>
                {/* <Link to={{state: }} tabIndex={tabIndex}
                    onClick={async () => {
                        await onClick();
                        await this.setStateAsync({ isMobileExpanded: false });
                        await this.doCloseElementRecursive(this.root);
                        await this.forceUpdateAsync();
                    }}
                >{text}</Link> */}
            </li>);
        } else {
            throw new Error('The \'to\' or \'onClick\' property must be defined, or else sub child items must be given.');
        }
    }

    public render(): React.ReactNode {
        console.warn("re-rendering nav menu", this.props.defaultItems[1].items?.[1]);
        console.warn("re-rendering nav menu ___ state", this.state);
        const { title, isFixed, colorTheme } = this.props;
        const { isMobileExpanded, isMobile, mobileTitleText,
            isMobileLeftArrowVisible,
            isMobileRightArrowVisible
        } = this.state;

        const cNavClasses = ['c-navbar'];
        if (isFixed) cNavClasses.push('f-fixed');
        if (isMobile) cNavClasses.push('f-mobile');
        if (isMobileExpanded) cNavClasses.push('f-opened');

        const isSelectedNav = this.state.currentNavUniqueId === this.root.uniqueId;
        const tabIndex = (isSelectedNav || !this.state.currentNavUniqueId) ? 0 : -1;

        return (
            <>
                <nav className={cNavClasses.join(' ')} role='navigation'>
                    <div className='f-header'>
                        <button
                            className='f-header-toggle' onClick={this.onClickToggleMobileMenu}
                            aria-expanded={this.state.isMobileExpanded} aria-label='Toggle Menu'>
                        </button>

                        <span>{title}</span>
                    </div>
                    <div className='f-mobile-title'>
                        {isMobileLeftArrowVisible && <button className='f-left x-hidden' aria-label='Navigate to parent menu' onClick={this.onClickMobilePrevButton} />}
                        <span>{mobileTitleText}</span>
                        {isMobileRightArrowVisible && <button className='f-right x-hidden' aria-label='Toggle Menu'></button>}
                    </div>

                    <div className={`f-nav-row f-nav-wrapper ${colorTheme} ${isSelectedNav ? 'f-current-nav' : ''}`} id={this.root.uniqueId}>
                        <button aria-expanded={this.root.val.isExpanded} tabIndex={tabIndex}
                            onClick={() => this.onClickSubMenuButton(this.root)}
                        >{this.root.val.text}</button>
                        {/* <button className='f-row-shift-left c-glyph glyph-chevron-left' aria-label='Previous menu items.'></button> */}
                        {this.root.children.length > 0 && (
                            <ul>
                                {this.root.children.map((item) => this.onRenderNavMenuItem(item, isSelectedNav))}
                            </ul>
                        )}
                        {/* <button className='f-row-shift-right c-glyph glyph-chevron-right' aria-label='Next menu items.'></button> */}
                    </div>
                </nav>
                {isFixed && <div className="c-navbar-fixed-spacer"></div>}
            </>
        );
    }
}



	// // ------  doShiftTab()  ----------------------------------------------------------------------
	// // Really only works if coming from the keydown listener.
	// component.doShiftTab = function(){
	// 	if ($(tkf.currFocus).is($('.c-navbar button[aria-expanded="true"]'))) {
	// 		tkf.Components.Navbar.closeElementRecursive($(tkf.currFocus));
	// 	}
	// };


	// // ------  doTab()  ---------------------------------------------------------------------------
	// // Really only works if coming from the keydown listener.
	// component.doTab = function(){
	// 	if (this.isMobile === true){
	// 		// If going forward, and you hit the end of a list, don't go past the end of the list.
	// 		if ($(tkf.currFocus).is('.c-navbar li:last-child > *:not(ul)')){
	// 			$(tkf.prevFocus).focus();
	// 		}
	// 	}else{
	// 		// If going forward, and you hit the end of a list, exit.
	// 		// if end of list is expanded, go INTO the list instead of quitting. 
	// 		if ($(tkf.currFocus).is($('.c-navbar li:last-child > *:not([aria-expanded="true"])'))){
	// 			// figure out where to put the focus next.
	// 			// Is parent's submenu expanded?
	// 			if ($(tkf.currFocus).closest('ul').siblings('button').is('[aria-expanded="true"]')){
	// 				tkf.Components.Navbar.closeElementRecursive($(tkf.currFocus).closest('ul').siblings('button'));

	// 				if ($(tkf.currFocus).is('.c-navbar li:last-child li:last-child > *')){
	// 					tkf.Components.Navbar.closeElementRecursive($(tkf.currFocus).closest('ul').parent().closest('ul').siblings('button'));

	// 					if ($(tkf.currFocus).is('.c-navbar li:last-child li:last-child li:last-child > *')){
	// 						tkf.Components.Navbar.closeElementRecursive($(tkf.currFocus).closest('ul').parent().closest('ul').parent().closest('ul').siblings('button'));
	// 					}
	// 				}
	// 			}
	// 		}
	// 	}
	// };

	// //</editor-fold>
