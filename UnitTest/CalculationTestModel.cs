namespace UnitTest
{
    public class CalculationTestModel
    {

        #region Properties

        public decimal[] Input { get; private set; }

        public decimal? AddOutput { get; private set; }

        public decimal? SubtractOutput { get; private set; }

        public decimal? MultiplyOutput { get; private set; }

        public decimal? DivideOutput { get; private set; }

        #endregion /Properties

        #region Constructors

        public CalculationTestModel(decimal[] input, 
            decimal? addOutput = null, decimal? subtractOutput = null, 
            decimal? multiplyOutput = null, decimal? divideOutput = null)
        {
            this.Input = input;
            this.AddOutput = addOutput;
            this.SubtractOutput = subtractOutput;
            this.MultiplyOutput = multiplyOutput;
            this.DivideOutput = divideOutput;
        }

        #endregion /Constructors

    }
}
