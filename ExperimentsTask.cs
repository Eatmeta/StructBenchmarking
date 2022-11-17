using System.Collections.Generic;

namespace StructBenchmarking
{
    public interface IExperimentsFactory
    {
        IClass CreateClassFactory();
        IStruct CreateStructFactory();
    }

    public class ArrayCreationExperimentsFactory : IExperimentsFactory
    {
        public IClass CreateClassFactory() => new ArrayCreationForClass();
        public IStruct CreateStructFactory() => new ArrayCreationForStruct();
    }

    public class MethodCallExperimentsFactory : IExperimentsFactory
    {
        public IClass CreateClassFactory() => new MethodCallForClass();
        public IStruct CreateStructFactory() => new MethodCallForStruct();
    }

    public interface IClass
    {
        List<ExperimentResult> DoExperimentsForClass(IBenchmark benchmark, int repetitionsCount);
    }

    public class ArrayCreationForClass : IClass
    {
        public List<ExperimentResult> DoExperimentsForClass(IBenchmark benchmark, int repetitionsCount)
        {
            var results = new List<ExperimentResult>();
            for (var i = 16; i <= 512; i *= 2)
                results.Add(
                    new ExperimentResult(i,
                    benchmark.MeasureDurationInMs(new ClassArrayCreationTask(i), repetitionsCount)));
            
            return results;
        }
    }

    public class MethodCallForClass : IClass
    {
        public List<ExperimentResult> DoExperimentsForClass(IBenchmark benchmark, int repetitionsCount)
        {
            var results = new List<ExperimentResult>();
            for (var i = 16; i <= 512; i *= 2)
                results.Add(new ExperimentResult(i,
                    benchmark.MeasureDurationInMs(new MethodCallWithClassArgumentTask(i), repetitionsCount)));
            
            return results;
        }
    }

    public interface IStruct
    {
        List<ExperimentResult> DoExperimentsForStruct(IBenchmark benchmark, int repetitionsCount);
    }

    public class ArrayCreationForStruct : IStruct
    {
        public List<ExperimentResult> DoExperimentsForStruct(IBenchmark benchmark, int repetitionsCount)
        {
            var results = new List<ExperimentResult>();
            for (var i = 16; i <= 512; i *= 2)
                results.Add(new ExperimentResult(i,
                    benchmark.MeasureDurationInMs(new StructArrayCreationTask(i), repetitionsCount)));
            
            return results;
        }
    }

    public class MethodCallForStruct : IStruct
    {
        public List<ExperimentResult> DoExperimentsForStruct(IBenchmark benchmark, int repetitionsCount)
        {
            var results = new List<ExperimentResult>();
            for (var i = 16; i <= 512; i *= 2)
                results.Add(new ExperimentResult(i,
                    benchmark.MeasureDurationInMs(new MethodCallWithStructArgumentTask(i), repetitionsCount)));
            
            return results;
        }
    }

    public class Experiments
    {
        private static readonly IExperimentsFactory ArrayCreationFactory = new ArrayCreationExperimentsFactory();
        private static readonly IExperimentsFactory MethodCallFactory = new MethodCallExperimentsFactory();

        public static ChartData BuildChartDataForArrayCreation(IBenchmark benchmark, int repetitionsCount)
        {
            return new ChartData
            {
                Title = "Create array",
                ClassPoints = ArrayCreationFactory.CreateClassFactory().DoExperimentsForClass(benchmark, repetitionsCount),
                StructPoints = ArrayCreationFactory.CreateStructFactory().DoExperimentsForStruct(benchmark, repetitionsCount)
            };
        }

        public static ChartData BuildChartDataForMethodCall(IBenchmark benchmark, int repetitionsCount)
        {
            return new ChartData
            {
                Title = "Call method with argument",
                ClassPoints = MethodCallFactory.CreateClassFactory().DoExperimentsForClass(benchmark, repetitionsCount),
                StructPoints = MethodCallFactory.CreateStructFactory().DoExperimentsForStruct(benchmark, repetitionsCount)
            };
        }
    }
}