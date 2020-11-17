namespace Build
{
    public class PipelineStep
    {

        #region Properties

        public PipelineOperation Operation { get;private set; }

        /// <summary>
        /// specifies if the pipeline can continue with or without success of this step
        /// </summary>
        public bool IsRequired { get; private set; }

        #endregion /Properties

        #region Constructors

        public PipelineStep(PipelineOperation operation, bool isRequired)
        {
            this.Operation = operation;
            this.IsRequired = isRequired;
        }

        #endregion /Constructors

    }
}
