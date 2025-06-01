import { delayMsAsync } from "./delay";

type SubscribeFunc = (isUnknownProgress: boolean, percent: number, msg?: string) => void;

export class ProgressX {
    private static cur: number = 0;
    private static msg?: string;
    private static isUnknownProgress: boolean = false;
    private static subscribers: Record<string, SubscribeFunc> = {};

    public static getMessage = () => ProgressX.msg;
    public static getPercent = () => ProgressX.cur;
    public static getIsUnknownProgress = () => ProgressX.isUnknownProgress;

    public static subscribe(key: string, func: SubscribeFunc) {
        ProgressX.subscribers[key] = func;
    }

    public static unsubscribe(key: string) {
        delete ProgressX.subscribers[key];
    }

    public static async reportUnknownProgressAsync(status?: string) {
        ProgressX.reportHelper(true, 100, status);
        await delayMsAsync(0);
    }

    public static async reportUnknownProgress(status: (string | undefined) = undefined) {
        ProgressX.reportHelper(true, 100, status);
    }

    public static async ReportAsync(percent: number, status: (string | undefined) = undefined) {
        ProgressX.reportHelper(false, percent, status);
        await delayMsAsync(0);
    }

    public static Report(percent: number, status: (string | undefined) = undefined) {
        ProgressX.reportHelper(false, percent, status);
    }

    private static reportHelper(unknownProgress: boolean, percent: number, status: (string | undefined) = undefined) {
        if (percent > 100) ProgressX.cur = 100;
        else if (percent < 0) ProgressX.cur = 0;
        else ProgressX.cur = percent;
        if (status !== undefined) {
            ProgressX.msg = status;
        }

        ProgressX.isUnknownProgress = unknownProgress;

        for (let key of Object.keys(ProgressX.subscribers)) {
            let valFunc = ProgressX.subscribers[key];
            if (!!valFunc) {
                valFunc(ProgressX.isUnknownProgress, ProgressX.cur, ProgressX.msg);
            }
        }
    }
}