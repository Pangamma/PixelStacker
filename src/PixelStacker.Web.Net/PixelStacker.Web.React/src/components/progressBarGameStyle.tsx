import React from "react";
import { RateLimiter } from "@/utils/rateLimiter";
import { setStateAsyncFactory } from "@utils/stateSetter";
import "./progressBarGameStyle.scss";
import { ProgressX } from "@/utils/progressX";

interface ProgressBarGameStyleProps {
}

interface ProgressBarGameStyleState {
    isLoading: boolean;
    message?: string;
    cur: number;
    max: number;
    isSpinning: boolean;
}

export class ProgressBarGameStyle extends React.PureComponent<
    ProgressBarGameStyleProps,
    ProgressBarGameStyleState
> {
    constructor(props: ProgressBarGameStyleProps) {
        super(props);
        this.state = {
            isLoading: true,
            isSpinning: ProgressX.getIsUnknownProgress(),
            max: 100,
            cur: ProgressX.getPercent(),
            message: ProgressX.getMessage() || 'Upload a file to get started.'
        };
    }

    // ------  doInitialize()  --------------------------------------------------------------------
    // Initialize any listeners here, any timers or repeating events as well.
    public async componentDidMount() {
        ProgressX.subscribe('progressBarGameStyle', (isUnknownProgress, percent, msg) => {
            this.setState({
                isSpinning: isUnknownProgress,
                cur: percent,
                message: msg
            });
        });
    }

    public componentWillUnmount() {
        ProgressX.unsubscribe('progressBarGameStyle');
    }

    public async componentDidUpdate(
        prevProps: ProgressBarGameStyleProps,
        prevState: ProgressBarGameStyleState
    ) {
    }

    public render() {
        const { cur, max, message, isSpinning } = this.state;
        const percent = cur / max * 100;

        return (
            <div className="progressBarGs">
                <div className="cc-container">
                    <div className={`cc-fillBar ${isSpinning && 'f-animated' || ''}`} style={{ width: `${percent}%` }}></div>
                </div>
                <div className="cc-message">{message || ''}</div>
            </div>
        );
    }
}