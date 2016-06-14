using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub;

namespace ZLSoft.ThirdInterface.Services
{
    public class WSSvrs : BaseService, IThirdService
    {
        public IList<StrObjectDict> DoService(StrObjectDict obj)
        {
            if (OtherProperties != null)
            {
                object isAsync = OtherProperties["IsAsync"];
                if("1".Equals(isAsync.ToString())){
                    //异步访问
                }
                else
                {
                    //同步访问
                    string serverUrl = OtherProperties["ServiceUrl"].ToString();
                    string inputTemplate = OtherProperties["InputTemplate"].ToString();
                    string outputTemplate = OtherProperties["OutputTemplate"].ToString();

                    //通过入参对象生成实际入参内容
                    //加载模板
                    Template template = Template.Parse(inputTemplate);//用模板内容做为参数解析得到Template对象
                    string result = template.Render(Hash.FromDictionary(obj));//用模板所需的元素做为参数呈现处理后的结果
                }


                return null;
            }
            else
            {
                return null;
            }
        }


        public IList<StrObjectDict> DoServiceAsync(StrObjectDict obj)
        {
            throw new NotImplementedException();
        }
    }
}
