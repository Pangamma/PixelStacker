namespace PixelStacker.IO.Config
{
    public class MaterialSelectOptions
    {
        public MaterialSelectOptions()
        {
        }

        /// <summary>
        /// TRUE if stained glass panes can be layered ontop of regular blocks
        /// </summary>
        public bool IsMultiLayer { get; set; } = true;

        /// <summary>
        /// TRUE if stained glass panes MUST be layered ontop of regular blocks
        /// </summary>
        public bool IsMultiLayerRequired { get; set; } = false;

    }
}