namespace aoc_dotnet.Year2024.Day9;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var diskMap = input[0].ToCharArray().Select(c => int.Parse("" + c)).ToArray();
        var blocks = diskMap.Where((_, i) => i % 2 == 0).ToList();
        var blockIndexes = Enumerable.Range(0, blocks.Count).ToList();
        var spaces = diskMap.Skip(1).Where((_, i) => i % 2 == 0).ToList();
        var fileIndex = 0;
        var trailingBlock = 0;
        var trailingBlockIndex = 0;
        var res = "";
        while (blocks.Count > 0)
        {
            var block = blocks.First();
            blocks.RemoveAt(0);
            res += string.Join("", Enumerable.Range(0, block).Select(_ => fileIndex));
            if (spaces.Count > 0)
            {
                var space = spaces.First();
                spaces.RemoveAt(0);
                while (space > 0)
                {
                    if (trailingBlock == 0)
                    {
                        trailingBlockIndex = blockIndexes.Last();
                        trailingBlock = blocks.Last();
                        blockIndexes.RemoveAt(blockIndexes.Count - 1);
                        blocks.RemoveAt(blocks.Count - 1);
                        if (spaces.Count > 0)
                        {
                            spaces.RemoveAt(spaces.Count - 1);
                        }
                    }

                    res += "" + trailingBlockIndex;
                    trailingBlock--;
                    space--;
                }
            }

            fileIndex++;
        }

        res += string.Join("", Enumerable.Range(0, trailingBlock).Select(_ => trailingBlockIndex));
        return "" + res.ToCharArray().Select((c, i) => long.Parse("" + c) * i).Sum();
    }

    public string Part2(string[] input)
    {
        var diskMap = input[0].ToCharArray()
            .Select((c, i) => (int.Parse("" + c), i % 2 == 0, i % 2 != 0 ? 0 : i / 2))
            .Where(v => v.Item1 > 0)
            .ToList();
        var blocks = diskMap.Where(v => v.Item2).ToList();
        while (blocks.Count > 0)
        {
            var block = blocks.Last();
            blocks.RemoveAt(blocks.Count - 1);
            var insertIndex = diskMap.FindIndex(s => !s.Item2 && s.Item1 >= block.Item1);
            if (insertIndex == -1) continue;
            var oldIndex = diskMap.FindIndex(b => b == block);
            if (oldIndex == -1 || oldIndex <= insertIndex) continue;
            ReplaceElement(diskMap, oldIndex);

            var totalSpace = diskMap[insertIndex].Item1;
            diskMap.RemoveAt(insertIndex);
            diskMap.Insert(insertIndex, block);
            if (totalSpace - block.Item1 > 0)
            {
                diskMap.Insert(insertIndex + 1, (totalSpace - block.Item1, false, 0));
                ReplaceElement(diskMap, insertIndex + 1);
            }
        }
        
        var idx = 0;
        long total = 0;
        foreach (var x in diskMap)
        {
            if (x.Item2)
            {
                total += Enumerable.Range(0, x.Item1).Select(y => (long)(idx + y) * x.Item3).Sum();
            }

            idx += x.Item1;
        }
        return "" + total;
    }

    private void ReplaceElement(List<(int, bool, int)> diskMap, int oldIndex)
    {
        var block = diskMap[oldIndex];
        var spaceToAdd = (block.Item1, false, 0);
        var toRemove = 1;
        var removeFromIndex = oldIndex;
        if (oldIndex - 1 >= 0)
        {
            var previous = diskMap[oldIndex - 1];
            if (!previous.Item2)
            {
                removeFromIndex--;
                toRemove++;
                spaceToAdd.Item1 += previous.Item1;
            }
        }

        if (oldIndex + 1 < diskMap.Count)
        {
            var next = diskMap[oldIndex + 1];
            if (!next.Item2)
            {
                toRemove++;
                spaceToAdd.Item1 += next.Item1;
            }
        }

        diskMap.RemoveRange(removeFromIndex, toRemove);
        diskMap.Insert(removeFromIndex, spaceToAdd);
    }
}