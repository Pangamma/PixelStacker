/**
 * Returns an interface stripped of all keys that don't resolve to U, defaulting 
 * to a non-strict comparison of T[key] extends U. Setting B to true performs
 * a strict type comparison of T[key] extends U & U extends T[key]
 */
// export type KeysOfType<T, U, B = false> = {
//   [P in keyof T]: B extends true 
//     ? T[P] extends U 
//       ? (U extends T[P] 
//         ? P 
//         : never)
//       : never
//     : T[P] extends U 
//       ? P 
//       : never;
// }[keyof T];

// export type KeysOfType<T, K, V> = K extends keyof T ? 
//   (T[K] extends V ? K: never)
// : never;

// let foo: KeysOfType<ProductOption, number | undefined> = 'id';
// let faz: PrimaryKey<ProductOption> = 'id';
// let zip: ProductOption = { isActive: true, id: 9, isQuantityEnabled: true, name: 'aaaa', optionGroupKey: 'AZSASA' };


export type PrimaryKey<T extends Record<string, unknown>> =  keyof T & KeysOfType<T, number | undefined>;
export type PickByValue<T, V> = Pick<T, { [K in keyof T]: T[K] extends V ? K : never }[keyof T]>;
export type KeysOfType<T, K> = keyof PickByValue<T, K>; // "includeMe" | "andMe"

// export type Filter<T, Allowed> = {
//   [P in keyof T]: T[P] extends Allowed ? P : never
// }[keyof T]