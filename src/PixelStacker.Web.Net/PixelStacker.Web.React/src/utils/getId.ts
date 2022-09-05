import { Logger } from "./logger";

const getIdIndexes: Record<string, number> = {};
export const getId = (prefix: string = ''): string => {
    if (getIdIndexes[prefix] === undefined) {
        getIdIndexes[prefix] = 0;
    }
    else if (getIdIndexes[prefix] > Number.MAX_SAFE_INTEGER - 5) {
        Logger.warn('getIdIndex is approaching the max value soon for "'+prefix+'" prefix! We have to roll back to lowest value!');
        getIdIndexes[prefix] = Number.MIN_SAFE_INTEGER;
    }
    
    return `${prefix || 'randomId'}${++getIdIndexes[prefix]}`;
}