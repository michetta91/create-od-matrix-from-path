namespace CreateMainZonesAccordingToStopPoint
{
    internal class Solver
    {

        public static Maps ComputeMaps(string inputFolder,
                                       string mergeAttributeName,
                                       HashSet<string> elementsToMerge)
        {
            var maps = new Maps();

            return maps;
        }

        private static Dictionary<int, ElementToMerge> ImportPathLegs(string inputFolder)
        {
            Dictionary<int, ElementToMerge> elementToMergeMap = new();

            var files = Directory.GetFiles(inputFolder).Where(file => Path.GetExtension(file) == ".att");
            foreach (var file in files)
            {
            }

            return elementToMergeMap;
        }
    }
}
