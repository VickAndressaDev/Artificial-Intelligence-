using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneticAlgorithmConsoleAPp
{
    public class Program
    {
        public static int CHROMOSOME_LENGTH = 10;
        public static int POPULATION_SIZE = 50;
        public static int SEED = 10;
        public static int MAX_GENERATION = 1000;
        public static float MUTATION_RATE = 0.02f;

        public static Random random = new Random(SEED);

        public static List<int[]> CreatePopulation()
        {

            List<int[]> population = new List<int[]>();
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                int[] chromosome = new int[CHROMOSOME_LENGTH];
                for (int j = 0; j < CHROMOSOME_LENGTH; j++)
                    chromosome[j] = random.Next(0, 2);
                population.Add(chromosome);

            }
            return population;
        }

        public static int ConvertArrayToDecimal(int[] chromosome)
        {
            int value = 0;
            for (int i = 0; i < CHROMOSOME_LENGTH; i++)
            {
                int exp = CHROMOSOME_LENGTH - i - 1;
                value += (int)(Math.Pow(2, exp) * chromosome[i]);
            }
            return value;
        }

        public static int[] EvaluateFitness(List<int[]> population)
        {
            int[] fitness = new int[POPULATION_SIZE];
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                int x = ConvertArrayToDecimal(population[i]);
                fitness[i] = x * x;

            }
            return fitness;
        }

        public static float[] createProbabilities(int[] fitness)
        {
            int sum = 0;
            for (int i = 0; i < POPULATION_SIZE; i++)
                sum += fitness[i];

            float[] prob = new float[POPULATION_SIZE];
            for (int i = 0; i < POPULATION_SIZE; i++)
                prob[i] = fitness[i] / (float)sum;

            return prob;
        }

        public static int[] Selection(List<int[]> population, float[] probabilities)
        {
            int[] parent = new int[CHROMOSOME_LENGTH];
            float roulleteNumber = (float)random.NextDouble();
            float probabilitySum = 0.0f;
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                probabilitySum += probabilities[i];
                if (roulleteNumber < probabilitySum)
                {
                    Array.Copy(population[i], parent, CHROMOSOME_LENGTH);
                    break;
                }
            }
            return parent;

        }

        public static void Crossover(int[] parent1, int[] parent2, int[] child1, int[] child2)
        {
            int cutOff = random.Next(1, CHROMOSOME_LENGTH);
            for (int i = 0; i < CHROMOSOME_LENGTH; i++)
            {
                child1[i] = i < cutOff ? parent1[i] : parent2[i];
                child2[i] = i < cutOff ? parent2[i] : parent1[i];
            }
        }

        public static void Mutation(int[] chromosome, float mutationRate)
        {
            if (random.NextDouble() < mutationRate)
            {
                int position = random.Next(0, CHROMOSOME_LENGTH);
                int alelo = chromosome[position];
                chromosome[position] = 1 - alelo;
            }
        }

        public static void Main(string[] args)
        {
            int generation = 0;
            List<int[]> population = CreatePopulation();
            int[] fitness = EvaluateFitness(population);
            while (generation++ < MAX_GENERATION)
            {
                List<int[]> newPopulation = new List<int[]>();
                float[] probabilities = createProbabilities(fitness);
                do
                {
                    int[] parent1 = Selection(population, probabilities);
                    int[] parent2 = Selection(population, probabilities);

                    int[] child1 = new int[CHROMOSOME_LENGTH];
                    int[] child2 = new int[CHROMOSOME_LENGTH];

                    Crossover(parent1, parent2, child1, child2);
                    Mutation(child1, MUTATION_RATE);
                    Mutation(child2, MUTATION_RATE);

                    newPopulation.Add(child1);
                    newPopulation.Add(child2);


                } while (newPopulation.Count() < POPULATION_SIZE);

                population = newPopulation;
                fitness = EvaluateFitness(population);
                Console.WriteLine("Generation: #{0:000}\t", generation);
                for (int i = 0; i < POPULATION_SIZE; i++)
                {
                    for (int j = 0; j < CHROMOSOME_LENGTH; j++)
                        Console.Write("{0}", population[i][j]);
                    Console.Write("\t{0:000}", fitness[i]);
                    Console.WriteLine("\t{0:000}", (int)(probabilities[i] * 100));

                }
            }
            Console.ReadKey();
        }
    }
}
