declare xmlStr xmltype:=xmltype('<IN>
select distinct a.病人Id "PatientID",a.主页id "HomePageID",a.姓名 "FullName",a.床号 as "BedNumber",a.状态 "Status",a.科室 as "Lesion",to_char(a.时间,''yyyy-mm-dd hh24:mi:ss'') as "ExcuteDate",
(select 诊断描述 from 病人诊断记录 b where a.病人id=b.病人id and a.主页id=b.主页id and b.诊断类型=2 and b.诊断次序=1 and b.记录来源=3) "Description",
(select substr(max(l.发生时间)||''|''||n.记录内容,instr(max(l.发生时间)||''|''||n.记录内容,''|'')+1) from 病人护理文件 P,病人护理数据 L,病人护理明细 N 
  where a.病人id=p.病人id and a.主页id=p.主页id and p.id=l.文件id and l.id=n.记录id and n.项目名称=''体温'' and
        l.发生时间 between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') 
        group by 记录内容) "BBT",
(select substr(max(l.发生时间)||''|''||n.记录内容,instr(max(l.发生时间)||''|''||n.记录内容,''|'')+1) from 病人护理文件 P,病人护理数据 L,病人护理明细 N 
  where a.病人id=p.病人id and a.主页id=p.主页id and p.id=l.文件id and l.id=n.记录id and n.项目名称=''呼吸'' and
        l.发生时间 between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') 
        group by 记录内容) "R",
(select substr(max(l.发生时间)||''|''||n.记录内容,instr(max(l.发生时间)||''|''||n.记录内容,''|'')+1) from 病人护理文件 P,病人护理数据 L,病人护理明细 N 
  where a.病人id=p.病人id and a.主页id=p.主页id and p.id=l.文件id and l.id=n.记录id and n.项目名称=''脉搏'' and
        l.发生时间 between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') 
        group by 记录内容) "P",
(select substr(max(l.发生时间)||''|''||n.记录内容,instr(max(l.发生时间)||''|''||n.记录内容,''|'')+1) from 病人护理文件 P,病人护理数据 L,病人护理明细 N 
  where a.病人id=p.病人id and a.主页id=p.主页id and p.id=l.文件id and l.id=n.记录id and n.项目名称=''收缩压'' and
        l.发生时间 between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') 
        group by 记录内容)||''/''||
(select substr(max(l.发生时间)||''|''||n.记录内容,instr(max(l.发生时间)||''|''||n.记录内容,''|'')+1) from 病人护理文件 P,病人护理数据 L,病人护理明细 N 
  where a.病人id=p.病人id and a.主页id=p.主页id and p.id=l.文件id and l.id=n.记录id and n.项目名称=''舒张压'' and
        l.发生时间 between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') 
        group by 记录内容) "BP",
(select substr(max(l.发生时间)||''|''||n.记录内容,instr(max(l.发生时间)||''|''||n.记录内容,''|'')+1) from 病人护理文件 P,病人护理数据 L,病人护理明细 N 
  where a.病人id=p.病人id and a.主页id=p.主页id and p.id=l.文件id and l.id=n.记录id and n.项目名称=''病情及护理摘要'' and
        l.发生时间 between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') 
        group by 记录内容) "NurseDigest"
from

(
--入院
select a.病人Id,a.主页id,a.姓名,a.出院病床 as 床号,''1'' as 状态,b.名称 as 科室 ,a.入院日期 as 时间 
from 病案主页 a ,部门表 b  
where a.当前病区id=''$LesionID$'' and a.入院日期 between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') and a.入院科室id=b.id 
union all
--出院
select a.病人Id,a.主页id,a.姓名,a.出院病床,''2'' as 状态,b.名称,a.出院日期
from 病案主页 a ,部门表 b  
where a.当前病区id=''$LesionID$'' and  a.出院日期 between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') and a.出院科室id=b.id

union all 
--转入出
select e.病人Id,e.主页id,e.姓名,decode(a.病区id,''$LesionID$'',a.床号,b.床号),decode(a.病区id,''$LesionID$'',''3'',''4''),decode(a.病区id,''$LesionID$'',d.名称,c.名称),a.终止时间
from 病人变动记录 a,病人变动记录 b,部门表 c,部门表 d,病案主页 e
where a.终止原因=3 and a.病人id=b.病人id and a.主页id=b.主页id and a.终止原因=b.开始原因 and a.终止时间=b.开始时间 and 
      a.终止时间 between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') and (a.病区id=''$LesionID$'' or b.病区id=''$LesionID$'') and a.科室id=c.id and 
      b.科室id=d.id and a.病人id=e.病人id and a.主页id=e.主页id 

union all 
--手术、分娩
Select distinct d.病人Id,d.主页id,d.姓名,d.出院病床,decode(a.项目名称,''分娩'',''6'',''7''),null,a.记录时间
From 病人护理明细 A, 病人护理数据 B, 病人护理文件 C, 病案主页 D
Where a.记录id = b.Id And b.文件id = c.Id And c.病人id = d.病人id And c.主页id = d.主页id and d.当前病区id = ''$LesionID$'' and 
      a.记录类型=4 And a.记录时间 between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'')

union all 
--发热
Select distinct d.病人Id,d.主页id,d.姓名,d.出院病床,''5'',null,null
From 病人护理明细 A, 病人护理数据 B, 病人护理文件 C, 病案主页 D
Where a.记录id = b.Id And b.文件id = c.Id And c.病人id = d.病人id And c.主页id = d.主页id and d.当前病区id = ''$LesionID$'' and 
      a.项目序号 = 1 And Zl_To_Number(a.记录内容, 0) &gt; 37.5 And a.记录时间&gt;to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'')-3

union all 
--死亡
select distinct a.病人Id,a.主页id,a.姓名,a.出院病床,''8'',c.名称,b.开始执行时间
from 病案主页 a ,病人医嘱记录 b,部门表 c,诊疗项目目录 d  
where a.病人id=b.病人id and a.主页id=b.主页id and a.当前病区id=''$LesionID$'' and b.执行科室id=c.id and 
      b.诊疗项目id=d.id and d.类别=''Z'' and d.操作类型=''11'' and b.开嘱时间 between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') and exists
      (select 1 from 病人医嘱发送 e where e.医嘱id=b.id) 

union all      
--9：病重  10：病危
select distinct a.病人Id,a.主页id,a.姓名,a.出院病床,d.操作类型,c.名称,b.开始执行时间
from 病案主页 a ,病人医嘱记录 b,部门表 c,诊疗项目目录 d 
where a.病人id=b.病人id and a.主页id=b.主页id and a.当前病区id=''$LesionID$'' and b.执行科室id=c.id and
      b.诊疗项目id=d.id and d.类别=''Z'' and d.操作类型 in(''9'',''10'') and b.医嘱状态 in(3,8,9) and 
      (b.开始执行时间 between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') or (b.开始执行时间&lt;to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and 
      (nvl(b.执行终止时间,to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'')+1/24/60/60)&gt;to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') or nvl(b.执行终止时间,to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'')+1/24/60/60) between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'')and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'')))) 
) a
</IN>');
begin
Insert into 系统_服务定义 (ID,名称,类型,说明,入参模板,出参模板,异步,数据源,是否作废,作废时间,修改用户,修改时间,是否本地存储,是否系统内置,对照对象ID,是否对照表对象) 
values ('48373493-109f-4af6-b9dd-64bf4166d345','交班报告',4,null,xmlStr,'<Out>{{Out}}</Out>',0,'ef090fc0-a178-42a7-81da-5f6d5021bf81',0,null,null,null,0,0,null,0);
end