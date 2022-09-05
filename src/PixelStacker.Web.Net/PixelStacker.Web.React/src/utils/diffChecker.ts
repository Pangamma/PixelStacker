// ------  isDiffFactory  -------------------------------------------------------------------------
/**
 * @param prevObj
 * @param nextObj
 * @returns A function that can be used to check if a property differs between two instances of the same type.
 */
export function isDiffFactory<T, K extends keyof T>(prevObj?: T, nextObj?: T):
  (propertyKey: K, customIsDiffFunc?: (prev?: any, next?: any) => boolean) => boolean {

  return (propertyKey: K, customIsDiffFunc?: (prev?: any, next?: any) => boolean) => {
    const objA = prevObj && prevObj[propertyKey];
    const objB = nextObj && nextObj[propertyKey];

    if (objA === undefined && objB === undefined) {
      return false;
    }

    if (objA === undefined || objB === undefined) {
      return true; // One is set, one is not. Therefore different.
    }

    if (isDiffPrimitiveProps(objA, objB)) {
      return true;
    }

    if (!!customIsDiffFunc) {
      if (customIsDiffFunc(objA, objB)) {
        return true;
      }
    }

    return false;
  };
}


// ------  isDiffPrimitiveProps  ------------------------------------------------------------------
/**
 * Performs shallow diff check to see if the primitive properties within the objects differ.
 * Objects and functions will not be compared.
 * @param prevObj
 * @param nextObj
 * @returns true if different
 */
export function isDiffPrimitiveProps<T>(prevObj: T, nextObj: T): boolean {
  if (!!prevObj !== !!nextObj) {
    return true;
  }

  for (const key in prevObj) {
    if (!(key in prevObj) || prevObj[key] !== nextObj[key]) {
      const typ = typeof (prevObj[key]);
      switch (typ) {
        case 'number':
        case 'boolean':
        case 'string':
        case 'undefined':
        case 'bigint':
          return true;

        case 'object':
        case 'function':
        default:
          break; // We don't want to compare functions or objects.
      }
    }
  }

  for (const key in nextObj) {
    if (!(key in nextObj) || nextObj[key] !== prevObj[key]) {
      const typ = typeof (nextObj[key]);
      switch (typ) {
        case 'number':
        case 'boolean':
        case 'string':
        case 'undefined':
        case 'bigint':
          return true;

        case 'object':
        case 'function':
        default:
          break; // We don't want to compare functions or objects.
      }
    }
  }

  return false;
}


// ------  isDiffArraysByValue  -------------------------------------------------------------------
/**
 * Performs shallow diff check to see if the primitive properties within the objects differ.
 * Objects and functions will not be compared.
 * @param prevObj
 * @param nextObj
 * @returns true if different
 */
export function isDiffArraysByValue<T>(
  prevObj: T[] | undefined,
  nextObj: T[] | undefined,
  areItemsDifferent: (a: T, b: T) => boolean
): boolean | undefined {
  if (prevObj === undefined && nextObj === undefined) {
    return false;
  }

  if ((prevObj !== undefined) !== (nextObj !== undefined)) {
    return true;
  }

  if ((prevObj !== undefined) && (nextObj !== undefined)) {
    if (prevObj.length !== nextObj.length) {
      return true;
    }

    for (let i = 0; i < prevObj.length; i++) {
      const A = prevObj[i];
      const B = nextObj[i];

      if (!!A !== !!B) {
        return true;
      }

      if (A !== undefined && B !== undefined) {
        if (areItemsDifferent(A, B)) {
          return true;
        }
      }
    }
  }

  return false;
}

// ------  deepClone  -----------------------------------------------------------------------------
/**
 * Deep copy function for TypeScript. Does not include functions.
 * @param T Generic type of target/copied value.
 * @param target Target value to be copied.
 */
export const deepClone = <T>(target: T): T => {
  const json = JSON.stringify(target);
  const output = JSON.parse(json, (propertyKey: string, propertyValue: unknown) => {
    // // This is needed to parse the Date objects that are serialized into json strings.
    // // Example of format: 2017-06-08T14:25:36.005Z
    // const regexDateISO = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*))(?:Z|(\+|-)([\d|:]*))?$/;
    // // Example format: /Date(1245398693390)/    <--- Yes, it can come this way via AJAX JSON for MS tech stacks.
    // const regexDateMsAjax = /^\/Date\((d|-|.*)\)[\/|\\]$/;

    // if (typeof propertyValue === 'string') {
    //   let match = regexDateISO.exec(propertyValue);
    //   if (!!match) {
    //     return new Date(propertyValue);
    //   }

    //   match = regexDateMsAjax.exec(propertyValue);
    //   if (!!match) {
    //     let matchGroups = match[1].split(/[-+,.]/);
    //     return new Date(matchGroups[0] ? +matchGroups[0] : 0 - +matchGroups[1]);
    //   }
    // }

    return propertyValue;
  }) as T;

  return output;
};

// ------  isDiffDeepProps  ------------------------------------------------------------------
/**
 * Performs deep diff check to see if object is the same if serialized to JSON when keys are sorted.
 * @param prevObj
 * @param nextObj
 * @returns true if different
 */
export function isDiffDeepProps<T>(prevObj: T, nextObj: T): boolean {
  if (prevObj === undefined && nextObj === undefined) {
    return false;
  }

  if ((prevObj !== undefined) !== (nextObj !== undefined)) {
    return true;
  }

  if (typeof (prevObj) !== typeof (nextObj)) {
    return true;
  }

  if (Array.isArray(prevObj) && Array.isArray(nextObj)) {
    if (isDiffArraysByValue(prevObj as unknown[], nextObj as unknown[], (a: unknown, b: unknown) => {
      return isDiffDeepProps(a, b);
    })) {
      return true;
    }
  }

  if (prevObj instanceof Date && nextObj instanceof Date) {
    if ((prevObj as Date).getTime() !== (nextObj as Date).getTime()) {
      return true;
    }
  }

  if (typeof prevObj === 'object' && typeof nextObj === 'object') {
    if (isDiffPrimitiveProps(prevObj, nextObj)) {
      return true;
    }
  }

  return prevObj !== nextObj;
}
