using System.Collections.Generic;

namespace Build
{
    class Program
    {
        static void Main(string[] args)
        {
            var steps = GetPipelineSteps();
            new PipelineExecuter(steps).RunPipeline();
        }

        private static IList<PipelineStep> GetPipelineSteps()
        {
            var steps = new List<PipelineStep>();
            steps.Add(new PipelineStep(PipelineOperation.Build, true));
            steps.Add(new PipelineStep(PipelineOperation.Test, true));
            steps.Add(new PipelineStep(PipelineOperation.Deploy, true));
            return steps;
        }

    }
}
