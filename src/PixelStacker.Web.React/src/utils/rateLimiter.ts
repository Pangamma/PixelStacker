/**
 * Use this for when you only want to initiate actions once an action stops
 * occuring for longer than "delayMs" milliseonds. Useful for scroll event
 * watchers and other operations when you want to limit how often heavier
 * operations can be triggerred.
 */
export class RateLimiter {
    private delayMs: number;
    private timeoutHandle?: number;
  
    constructor(delayMs: number) {
      this.delayMs = delayMs;
    }
  
    public tryAction(action: () => void): RateLimiter {
      if (this.timeoutHandle !== undefined) {
        window.clearTimeout(this.timeoutHandle);
        this.timeoutHandle = undefined;
      }
  
      this.timeoutHandle = window.setTimeout(action, this.delayMs);
      return this;
    }
  
    public cancel(): RateLimiter {
      if (this.timeoutHandle !== undefined) {
        window.clearTimeout(this.timeoutHandle);
      }
  
      return this;
    }
  }
  