﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using OnTheFlyStats;

namespace ConnectionCore
{


    public class ConnectionNodeViewModel : INotifyPropertyChanged
    {
        private Stats stats = new OnTheFlyStats.Stats();
        public ObservableCollection<dynamic> Values { get; } = new ObservableCollection<dynamic>();
        private Subject<double> oneStream = new Subject<double>();
        private Subject<double> twoStream = new Subject<double>();


        public double Mean => stats.Average;

        public double StandardDeviation => stats.PopulationStandardDeviation;

        //DoubleItem doubleItem = new DoubleItem();

        public event PropertyChangedEventHandler PropertyChanged;

        public ConnectionNodeViewModel()
        {
            //doubleItem.Stopped += DoubleItem_Stopped;
            oneStream.Zip(twoStream, (a, b) => (a, b))
                .Subscribe(c =>
                 {
                     Values.Add(new { X = c.a, Y = c.b });
                     if (c.b != 0)
                     {
                         stats.Update(c.a / c.b);

                         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mean)));
                         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StandardDeviation)));
                     }
                 });
        }

        public bool IsNegative { get; set; }


        //private void DoubleItem_Stopped()
        //{
        //    stats.Update(doubleItem.One / doubleItem.Two);
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mean)));
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StandardDeviation)));

        //    Values.Add(new { X= doubleItem.One, Y=doubleItem.Two });

        //    doubleItem.Stopped -= DoubleItem_Stopped;
        //    doubleItem = new DoubleItem();
        //    doubleItem.Stopped += DoubleItem_Stopped;
        //}

        public void OnNext1(double one)
        {
            oneStream.OnNext(one);
        }


        public void OnNext2(double two)
        {
            twoStream.OnNext(two);
        }

    }



    public class DoubleItem
    {
        readonly SetUntil one = new SetUntil();
        readonly SetUntil two = new SetUntil();
        bool b_one = false;
        bool b_two = false;

        public double One => one.Value;

        public double Two => two.Value;


        public DoubleItem()
        {
            one.Stopped += One_Stopped;
            two.Stopped += Two_Stopped;
        }

        private void Two_Stopped()
        {
            b_two = true;
            if (b_one)
                Stopped.Invoke();
        }

        public event Action Stopped;

        private void One_Stopped()
        {
            b_one = true;
            if (b_two)
                Stopped.Invoke();
        }



        public void OnNext1(double one)
        {
            this.one.OnNext(one);
            two.Stop();

        }

        public void OnNext2(double two)
        {
            this.two.OnNext(two);
            one.Stop();
        }
    }



    public class SetUntil
    {
        bool stop = false;
        private readonly OnTheFlyStats.Stats list;

        public SetUntil()
        {
            list = new OnTheFlyStats.Stats();
        }

        public void Stop()
        {
            stop = true;
        }

        public event Action Stopped;

        public void OnNext(double value)
        {
            if (stop == true && list.N > 0)
                this.Stopped?.Invoke();
            else
                list.Update(value);
        }

        public double Value => list.Average;

    }
}
