using System.Numerics;

namespace aoc_dotnet.Year2018.Day13;

internal record struct Cart(Complex Position, Complex Direction, Complex NextTurn);

public class Solver: SolverInterface
{
    private Complex Up = -Complex.ImaginaryOne;
    private Complex TurnRight = Complex.ImaginaryOne;
    
    public string Part1(string[] input)
    {
        var (grid, carts) = ParseInput(input);
        while (true)
        {
            carts = carts.OrderBy(c => c.Position.Imaginary).ThenBy(c => c.Position.Real).ToList();
            var nextCarts = new List<Cart>();
            while (carts.Count > 0)
            {
                var cart = carts.First();
                var nextPos = cart.Position + cart.Direction;
                if (!grid.TryGetValue(nextPos, out _)) throw new Exception($"Went off the grid! ({nextPos.Imaginary}, {nextPos.Real})");
                var nextTurn = cart.NextTurn;
                var nextDirection = cart.Direction;
                if (grid[nextPos] is '+')
                {
                    nextDirection *= nextTurn;
                    nextTurn = new Dictionary<Complex, Complex>
                    {
                        [Complex.One] = TurnRight,
                        [TurnRight] = -TurnRight,
                        [-TurnRight] = 1,
                    }[nextTurn];
                } else if (grid[nextPos] is '/')
                {
                    nextDirection *= (nextDirection.Real != 0 ? -TurnRight : TurnRight);
                } else if (grid[nextPos] is '\\')
                {
                    nextDirection *= nextDirection.Real != 0 ? TurnRight : -TurnRight;
                }

                if (nextCarts.Any(c => c.Position == nextPos) || carts.Any(c => c.Position == nextPos))
                {
                    return $"{nextPos.Real},{nextPos.Imaginary}";
                }
                nextCarts.Add(new Cart(nextPos, nextDirection, nextTurn));
                carts.Remove(cart);
            }
            carts = nextCarts;
        }
    }

    public string Part2(string[] input)
    {
        var (grid, carts) = ParseInput(input);
        while (carts.Count > 1)
        {
            carts = carts.OrderBy(c => c.Position.Imaginary).ThenBy(c => c.Position.Real).ToList();
            var nextCarts = new List<Cart>();
            while (carts.Count > 0)
            {
                var cart = carts.First();
                carts.Remove(cart);
                var nextPos = cart.Position + cart.Direction;
                if (!grid.TryGetValue(nextPos, out _)) throw new Exception($"Went off the grid! ({nextPos.Imaginary}, {nextPos.Real})");
                var nextTurn = cart.NextTurn;
                var nextDirection = cart.Direction;
                if (grid[nextPos] is '+')
                {
                    nextDirection *= nextTurn;
                    nextTurn = new Dictionary<Complex, Complex>
                    {
                        [Complex.One] = TurnRight,
                        [TurnRight] = -TurnRight,
                        [-TurnRight] = 1,
                    }[nextTurn];
                } else if (grid[nextPos] is '/')
                {
                    nextDirection *= (nextDirection.Real != 0 ? -TurnRight : TurnRight);
                } else if (grid[nextPos] is '\\')
                {
                    nextDirection *= nextDirection.Real != 0 ? TurnRight : -TurnRight;
                }

                if (nextCarts.Any(c => c.Position == nextPos) || carts.Any(c => c.Position == nextPos))
                {
                    carts = carts.Where(c => c.Position != nextPos).ToList();
                    nextCarts = nextCarts.Where(c => c.Position != nextPos).ToList();
                    continue;
                }
                nextCarts.Add(new Cart(nextPos, nextDirection, nextTurn));
            }
            carts = nextCarts;
        }
        if (carts.Count == 1) return $"{carts.First().Position.Real},{carts.First().Position.Imaginary}";
        return "Ran out of carts!";
    }

    private (Dictionary<Complex, char>, List<Cart>) ParseInput(string[] input)
    {
        var grid = new Dictionary<Complex, char>();
        var carts = new List<Cart>();
        foreach (var y in Enumerable.Range(0, input.Length))
        {
            foreach (var x in Enumerable.Range(0, input[y].Length))
            {
                var coord = -Up * y + x;
                var c = input[y][x];
                switch (c)
                {
                    case '<' or '>':
                        carts.Add(new Cart(coord, c == '<' ? -1 : 1, -TurnRight));
                        grid.Add(coord, '-');
                        break;
                    case '^' or 'v':
                        carts.Add(new Cart(coord, c == '^' ? Up : -Up, -TurnRight));
                        grid.Add(coord, '|');
                        break;
                    case '|' or '-' or '+' or '/' or '\\':
                        grid.Add(coord, c);
                        break;
                }
            }
        }

        return (grid, carts);
    }
}