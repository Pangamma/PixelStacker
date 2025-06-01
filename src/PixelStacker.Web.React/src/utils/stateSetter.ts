
import { Component } from 'react';
/* eslint-disable */
/**
 * Developed to fix the 'You must not update state when component is not
 * mounted. ' errors that we get when our API calls finish and then try
 * to set state right afterwards, but the page component itself has already
 * been redirected to a different component. By using this function you are
 * agreeing to always check the rules of use to make sure you are not covering
 * up any memory leak code.
 *
 * There are some dangers if you use this method incorrectly. As long as you
 * follow the rules and are careful you should be fine.
 *
 * 1. Always make sure any event listeners registered get unregistered during
 *    the componentWillUnmount method of your component. Clean up after yourself!
 *
 * 2. Any repeating interval timers or tasks need to be cancelled when unloading.
 *
 * 3. Call this method ONCE from the constructor of your component.
 *    It will hook into the mount and unmount events to track an internal
 *    isMounted variable. When isMounted is false, setState will do nothing.
 *    Oh yeah. This will override setState.
 *
 * Takes care of method binding for the wrapped methods automatically.
 *
 * @param component
 */
export const disableUnmountedStateSetting = <P, S>(component: Component<P, S>): void => {
    let isMounted = false;
    if (component === undefined) {
        return;
    }

    // Override and wrap the original didMount method so we can get the isMounted = true step.
    const originalDidMount = component!.componentDidMount && component.componentDidMount.bind(component);
    component.componentDidMount = (async () => {
        isMounted = true;
        if (!!originalDidMount) {
            await originalDidMount();
        }
    }).bind(component);

    // Override and wrap the original didUnmount method so we can get the isMounted = false step.
    const originalWillUnmount = component!.componentWillUnmount && component.componentWillUnmount.bind(component);
    component.componentWillUnmount = (async () => {
        if (!!originalWillUnmount) {
            await originalWillUnmount();
        }
        isMounted = false;
    }).bind(component);

    // Override setState so it uses the isMounted checks.
    const originalSetState = component!.setState && component.setState.bind(component);
    component.setState = (<K extends keyof S>(
        state: ((prevState: Readonly<S>, props: Readonly<P>) => (Pick<S, K> | S | null)) | (Pick<S, K> | S | null),
        callback?: () => void
    ): void => {
        if (isMounted) {
            originalSetState(state, callback);
        }
    }).bind(component);
};

/**
 * Allows you to await setState calls rather than use the callback function
 * Declaration: private setStateAsync = setStateAsyncFactory(this);
 * Usage: await this.setStateAsync({myProp: val});
 * @param component
 */
export const setStateAsyncFactory = <P, S>(
    component: React.Component<P, S>
) => {
    return <K extends keyof S>(state:
        ((prevState: Readonly<S>, props: Readonly<P>) => (Pick<S, K> | S | null)) |
        Pick<S, K> | S | null) => {
        return new Promise<void>(resolve => component.setState(state, resolve));
    };
};

/**
 * Allows you to await setState calls rather than use the callback function
 * Usage: await this.setStateAsync(component, {myProp: val});
 * @param component
 * 
 */
export const setStateAsyncDirect = <P, S, K extends keyof S>(
    component: React.Component<P, S>,
    state:
        ((prevState: Readonly<S>, props: Readonly<P>) => (Pick<S, K> | S | null)) |
        Pick<S, K> | S | null
) => new Promise<void>(resolve => component.setState(state, resolve));

export const forceUpdateAsync = (component: React.Component) => new Promise<void>(resolve => component.forceUpdate(resolve));
export const forceUpdateAsyncFactory = (component: React.Component) => () => new Promise<void>(resolve => component.forceUpdate(resolve));
export const delayAsync = (ms: number) => new Promise(resolve => window.setTimeout(resolve, ms));
/* eslint-enable */
