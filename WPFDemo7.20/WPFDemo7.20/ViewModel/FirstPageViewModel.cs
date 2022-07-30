using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo7._20.Common;
using WPFDemo7._20.DataAccess;
using WPFDemo7._20.Model;

namespace WPFDemo7._20.ViewModel
{
    public class FirstPageViewModel:NotifyBase
    {
        private int _instrumentValue;

        public int InstrumentValue
        {
            get { return _instrumentValue; }
            set { _instrumentValue = value; this.DoNotify(); }
        }
        //表示一个动态数据收集，该集合在添加或删除项或刷新整个列表时提供通知。WPF提供的将 INotifyCollectionChanged 接口内置实现的动态集合
        public ObservableCollection<CourseSeriesModel> CourseSeriesList { get; set; } = new ObservableCollection<CourseSeriesModel>();
        Random random=new Random();
        bool TaskSwitch =true;
        List<Task> TasksList= new List<Task>();
        public FirstPageViewModel()
        {
            this.RefreshInstrumentValue();
            this.InitCourseSeries();
        }
        private int _itemCount;

        public int ItemCount
        {
            get { return _itemCount; }
            set { _itemCount = value; this.DoNotify(); }
        }

        private void InitCourseSeries()
        {
            
            var cList = LoaclDataAccess.Instance().GetCoursePlayRecord();
            this.ItemCount = cList.Max(c => c.SeriesList.Count);
            foreach (var item in cList)
            {
                this.CourseSeriesList.Add(item);
            }
        }
        private void RefreshInstrumentValue()
        {
          var task=Task.Factory.StartNew(new Action(async () =>
            {
            while (TaskSwitch)
            {
                InstrumentValue = random.Next(Math.Max(InstrumentValue - 5, -10), Math.Min(InstrumentValue + 5, 90));
                await Task.Delay(1000);
                }
            }));
            TasksList.Add(task);
        }
        public void Disposp()
        {
            try
            {
                TaskSwitch = false;
                Task.WaitAll(this.TasksList.ToArray());
            }
            catch
            {

            }
        }

    }
}
