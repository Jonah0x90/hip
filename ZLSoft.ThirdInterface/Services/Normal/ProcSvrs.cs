using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.THIRD;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.ThirdInterface.Services
{
    public class ProcSvrs : BaseService, IThirdService
    {
        public IList<StrObjectDict> DoService(StrObjectDict obj)
        {
            IList<object> list = new List<object>();
            list.Add(new
            {
                ID = 13,
                ExtendValue = "13被修改了"
            });

            list.Add(new
            {
                ID = 14,
                ExtendValue = "新增了14"
            });
            list.Add(new
            {
                ID = 11,
                ExtendValue = "11被修改了"
            });

            DB.Execute(new DBState
            {
                Name = "BatchExtendExecuteNoneQuery2",
                Param = list,
                Type = ESqlType.UPDATE
            });


            return null;
        }

        public StrObjectDict AsynDoService(Pub.StrObjectDict obj)
        {
            throw new NotImplementedException();
        }


        public IList<StrObjectDict> DoServiceAsync(StrObjectDict obj)
        {
            throw new NotImplementedException();
        }
    }
}
