using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using WPFDemo7._20.DataAccess.DataEntity;
using System.Data;
using WPFDemo7._20.Common;
using WPFDemo7._20.Model;
using LiveCharts;
using LiveCharts.Defaults;

namespace WPFDemo7._20.DataAccess
{
    public class LoaclDataAccess
    {
        private static LoaclDataAccess instance;
        private LoaclDataAccess() { }
        public static LoaclDataAccess Instance()
        {
            return instance??(instance=new LoaclDataAccess());
        }
        SqlConnection conn;
        //表示要对 SQL Server 数据库执行的一个 Transact-SQL 语句或存储过程。 此类不能被继承
        SqlCommand comm;
        //SqlDataAdapter是 DataSet和 SQL Server之间的桥接器，用于检索和保存数据
        SqlDataAdapter adapter;
        private void Dispose()
        {
            if (adapter != null)
            {
                adapter.Dispose();
                adapter = null;
            }
            if (comm!=null)
            {
                comm.Dispose();
                comm = null;
            }
            if (conn!= null)
            {
                conn.Close();
                conn.Dispose();
                conn =null;
            }
        }
        private bool DBConnection()
        {
            // string connStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            string connStr = "Server=localhost;Database=zx_data;user id=sa;pwd=19981126";
            if (conn==null)
            {
                conn = new SqlConnection(connStr);
            }
            try
            {
                conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public UserEntity CheckUserInfo(string userName, string pwd)
        {
            try 
            {
                if (DBConnection())
                {
                    string usersql = "select* from users where user_name=@user_name " +
                        "and password=@pwd and is_validation=1";
                    adapter = new SqlDataAdapter(usersql, conn);
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@user_name", SqlDbType.VarChar) { Value = userName });
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@pwd", SqlDbType.VarChar) { Value =MD5Provider.GetMD5String(pwd+"@"+userName) });

                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);
                    if (count <= 0)
                    {
                        throw new Exception("用户名和密码不正确！");
                    }

                    DataRow dr = table.Rows[0];
                    if (dr.Field<Int32>("is_can_Login")==0)
                    {
                        throw new Exception("当前用户没有权限使用此平台！");
                    }
                    UserEntity userInfo = new UserEntity();
                    userInfo.UserName = dr.Field<string>("user_name");
                    userInfo.RealName = dr.Field<string>("real_name");
                    userInfo.Password = dr.Field<string>("password");
                    userInfo.Avatar = dr.Field<string>("avatar");
                    userInfo.Gender = dr.Field<Int32>("gender");
                    return userInfo;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
            return null;
        }
        public List<CourseSeriesModel> GetCoursePlayRecord()
        {
            try
            {
                List<CourseSeriesModel> cModelList = new List<CourseSeriesModel>();
                if (DBConnection())
                {
                    string usersql = @"select a.course_name,a.course_id,b.play_count,b.is_growing,b.growing_rate,c.platform_name
                                    from courses a
                                    left
                                    join play_record b
                                    on a.course_id = b.course_id
                                    left Join platforms c
                                    on b.platform_id = c.platform_id
                                    order by a.course_id,c.platform_id";
                    adapter = new SqlDataAdapter(usersql, conn);
                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);
                    string courseId = "";
                    CourseSeriesModel cModel = null;
                    foreach (DataRow item in table.AsEnumerable())
                    {
                        string tempId = item.Field<string>("course_id");
                        if (courseId!=tempId)
                        {
                            courseId= tempId;
                            cModel = new CourseSeriesModel();
                            cModelList.Add(cModel);
                            cModel.CourseName = item.Field<string>("course_name");
                            cModel.SeriesColection = new LiveCharts.SeriesCollection();
                            cModel.SeriesList = new System.Collections.ObjectModel.ObservableCollection<SeriesModel>();
                        }
                        if (cModel!=null)
                        {
                            cModel.SeriesColection.Add(new LiveCharts.Wpf.PieSeries
                            {
                                Title = item.Field<string>("platform_name"),
                                Values = new ChartValues<ObservableValue>
                                   {
                                       new ObservableValue((double)item.Field<decimal>("play_count"))
                                   },
                                DataLabels = false
                            });
                            cModel.SeriesList.Add(new SeriesModel
                            {
                                SeriesName = item.Field<string>("platform_name"),
                                CurrentValue =item.Field<decimal>("play_count"),
                                IsGrowing=item.Field<Int32>("is_growing")==1,
                                ChangeRate=(int)item.Field<decimal>("growing_rate")
                            });
                        }
                    }
                }
                return cModelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
        }
    }
}
