﻿namespace FluentScheduler.Tests.TestApplication
{
    using LLibrary;
    using System;
    using System.Threading;

    public class MyRegistry : Registry
    {
        public MyRegistry()
        {
            NonReentrantAsDefault();

            Welcome();

            NonReentrant();
            Reentrant();

            OnceIn();
            OnceAt();

            Sleepy();
            Faulty();
            Removed();
            Parameter();
            Disposable();

            TenMinutes();
            Hour();
            Day();
            Weekday();
            Week();
        }

        private void Welcome()
        {
            Schedule(() => Console.Write("3, "))
                .WithName("[welcome]")
                .AndThen(() => Console.Write("2, "))
                .AndThen(() => Console.Write("1, "))
                .AndThen(() => Console.WriteLine("Live!"))
                .AndThen(() => Console.WriteLine("{0}You can check what's happening in the log file at \"{1}\"",
                    Environment.NewLine, L.Directory));
        }
        private void NonReentrant()
        {
            L.Register("[non reentrant]");

            Schedule(() =>
            {
                L.Log("[non reentrant]", "Sleeping a minute...");
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }).NonReentrant().WithName("[non reentrant]").ToRunEvery(1).Seconds();
        }

        private void Reentrant()
        {
            L.Register("[reentrant]");

            Schedule(() =>
            {
                L.Log("[reentrant]", "Sleeping a minute...");
                Thread.Sleep(TimeSpan.FromMinutes(3));
            }).WithName("[reentrant]").ToRunNow().AndEvery(1).Minutes();
        }

        private void OnceIn()
        {
            L.Register("[once in]");

            Schedule(() =>
            {
                JobManager.RemoveJob("[reentrant]");
                JobManager.RemoveJob("[non reentrant]");
                L.Log("[once in]", "Disabled the reentrant and non reentrant jobs.");
            }).WithName("[once in]").ToRunOnceIn(3).Minutes();
        }

        private void OnceAt()
        {
            L.Register("[once at]");

            Schedule(() => L.Log("[once at]", "It's almost midnight."))
                .WithName("[once at]").ToRunOnceAt(23, 50);
        }

        private void Sleepy()
        {
            L.Register("[sleepy]");

            Schedule(() =>
            {
                L.Log("[sleepy]", "Sleeping...");
                Thread.Sleep(new TimeSpan(0, 7, 30));
            }).WithName("[sleepy]").ToRunEvery(15).Minutes();
        }

        private void Faulty()
        {
            L.Register("[faulty]");

            Schedule(() =>
            {
                L.Register("[faulty]", "I'm going to raise an exception!");
                throw new Exception("I warned you.");
            }).WithName("[faulty]").ToRunEvery(20).Minutes();
        }

        private void Removed()
        {
            L.Register("[removed]");

            Schedule(() =>
            {
                L.Register("[removed]", "SOMETHING WENT WRONG.");
            }).WithName("[removed]").ToRunOnceIn(2).Minutes();
        }

        private void Parameter()
        {
            Schedule(new ParameterJob { Parameter = "Foo" }).WithName("[parameter]").ToRunOnceIn(10).Seconds();
        }

        private void Disposable()
        {
            Schedule<DisposableJob>().WithName("[disposable").ToRunOnceIn(10).Seconds();
        }

        private void TenMinutes()
        {
            L.Register("[ten minutes]");

            Schedule(() => L.Log("[ten minutes]", "Ten minutes has passed."))
                .WithName("[ten minutes]").ToRunEvery(10).Minutes();
        }

        private void Hour()
        {
            L.Register("[hour]");

            Schedule(() => L.Log("[hour]", "A hour has passed."))
                .WithName("[hour]").ToRunEvery(1).Hours();
        }

        private void Day()
        {
            L.Register("[day]");

            Schedule(() => L.Log("[day]", "A day has passed."))
                .WithName("[day]").ToRunEvery(1).Days();
        }

        private void Weekday()
        {
            L.Register("[weekday]");

            Schedule(() => L.Log("[weekday]", "A new weekday has started."))
                .WithName("[weekday]").ToRunEvery(1).Weekdays();
        }

        private void Week()
        {
            L.Register("[week]");

            Schedule(() => L.Log("[week]", "A new week has started."))
                .WithName("[week]").ToRunEvery(1).Weeks();
        }
    }
}
