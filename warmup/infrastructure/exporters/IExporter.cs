namespace warmup.infrastructure.exporters
{
    internal interface IExporter
    {
        void Export(string sourceControlWarmupLocation, string templateName, TargetDir targetDir);
    }
}