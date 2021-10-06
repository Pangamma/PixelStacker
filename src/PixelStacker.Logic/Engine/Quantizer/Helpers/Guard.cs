using System;

namespace SimplePaletteQuantizer.Helpers
{
    public static class Guard
    {
        /// <summary>
        /// Checks if an argument is null
        /// </summary>
        /// <param name="argument">argument</param>
        /// <param name="argumentName">argument name</param>
        public static void CheckNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                string message = string.Format("Cannot use '{0}' when it is null!", argumentName);
                throw new ArgumentNullException(message);
            }
        }
    }
}
