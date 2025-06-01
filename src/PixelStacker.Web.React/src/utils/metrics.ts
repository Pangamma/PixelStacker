
// import { Analytics, AnalyticsPlugin}  from 'analytics';
export type MetricDimensionValue = string | number | boolean;
export class Metrics {
    // private static analytics = Analytics({
    //     app: 'pixelstacker-web',
    //     plugins: [
    //         googleAnalytics({
    //             measurementIds: ['UA-172410223-1']
    //         })
    //     ]
    // });

    public static track(metricName: string, value: number, dimensions: Record<string, MetricDimensionValue>) {
        // Metrics.analytics.track(metricName, dimensions);
    }

    public static async trackDependencyAsync<T>(metricName: string, dimensions: Record<string, MetricDimensionValue>, func: () => Promise<T>): Promise<T> {
        try {
            const rs = await func();
            Metrics.track(metricName, 1, dimensions);
            return rs;
        } catch (e) {
            Metrics.track(metricName, 0, dimensions);
            return Promise.reject(e);
        }
    }
    
    // /* Track a page view */
    // analytics.page()

    // /* Track a custom event */
    // analytics.track('cartCheckout', {
    //         item: 'pink socks',
    //         price: 20
    // })

    // /* Identify a visitor */
    // analytics.identify('user-id-xyz', {
    //             firstName: 'bill',
    //             lastName: 'murray'
    // })
}