declare xmlStr xmltype:=xmltype('<IN>
select distinct a.����Id "PatientID",a.��ҳid "HomePageID",a.���� "FullName",a.���� as "BedNumber",a.״̬ "Status",a.���� as "Lesion",to_char(a.ʱ��,''yyyy-mm-dd hh24:mi:ss'') as "ExcuteDate",
(select ������� from ������ϼ�¼ b where a.����id=b.����id and a.��ҳid=b.��ҳid and b.�������=2 and b.��ϴ���=1 and b.��¼��Դ=3) "Description",
(select substr(max(l.����ʱ��)||''|''||n.��¼����,instr(max(l.����ʱ��)||''|''||n.��¼����,''|'')+1) from ���˻����ļ� P,���˻������� L,���˻�����ϸ N 
  where a.����id=p.����id and a.��ҳid=p.��ҳid and p.id=l.�ļ�id and l.id=n.��¼id and n.��Ŀ����=''����'' and
        l.����ʱ�� between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') 
        group by ��¼����) "BBT",
(select substr(max(l.����ʱ��)||''|''||n.��¼����,instr(max(l.����ʱ��)||''|''||n.��¼����,''|'')+1) from ���˻����ļ� P,���˻������� L,���˻�����ϸ N 
  where a.����id=p.����id and a.��ҳid=p.��ҳid and p.id=l.�ļ�id and l.id=n.��¼id and n.��Ŀ����=''����'' and
        l.����ʱ�� between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') 
        group by ��¼����) "R",
(select substr(max(l.����ʱ��)||''|''||n.��¼����,instr(max(l.����ʱ��)||''|''||n.��¼����,''|'')+1) from ���˻����ļ� P,���˻������� L,���˻�����ϸ N 
  where a.����id=p.����id and a.��ҳid=p.��ҳid and p.id=l.�ļ�id and l.id=n.��¼id and n.��Ŀ����=''����'' and
        l.����ʱ�� between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') 
        group by ��¼����) "P",
(select substr(max(l.����ʱ��)||''|''||n.��¼����,instr(max(l.����ʱ��)||''|''||n.��¼����,''|'')+1) from ���˻����ļ� P,���˻������� L,���˻�����ϸ N 
  where a.����id=p.����id and a.��ҳid=p.��ҳid and p.id=l.�ļ�id and l.id=n.��¼id and n.��Ŀ����=''����ѹ'' and
        l.����ʱ�� between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') 
        group by ��¼����)||''/''||
(select substr(max(l.����ʱ��)||''|''||n.��¼����,instr(max(l.����ʱ��)||''|''||n.��¼����,''|'')+1) from ���˻����ļ� P,���˻������� L,���˻�����ϸ N 
  where a.����id=p.����id and a.��ҳid=p.��ҳid and p.id=l.�ļ�id and l.id=n.��¼id and n.��Ŀ����=''����ѹ'' and
        l.����ʱ�� between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') 
        group by ��¼����) "BP",
(select substr(max(l.����ʱ��)||''|''||n.��¼����,instr(max(l.����ʱ��)||''|''||n.��¼����,''|'')+1) from ���˻����ļ� P,���˻������� L,���˻�����ϸ N 
  where a.����id=p.����id and a.��ҳid=p.��ҳid and p.id=l.�ļ�id and l.id=n.��¼id and n.��Ŀ����=''���鼰����ժҪ'' and
        l.����ʱ�� between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') 
        group by ��¼����) "NurseDigest"
from

(
--��Ժ
select a.����Id,a.��ҳid,a.����,a.��Ժ���� as ����,''1'' as ״̬,b.���� as ���� ,a.��Ժ���� as ʱ�� 
from ������ҳ a ,���ű� b  
where a.��ǰ����id=''$LesionID$'' and a.��Ժ���� between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') and a.��Ժ����id=b.id 
union all
--��Ժ
select a.����Id,a.��ҳid,a.����,a.��Ժ����,''2'' as ״̬,b.����,a.��Ժ����
from ������ҳ a ,���ű� b  
where a.��ǰ����id=''$LesionID$'' and  a.��Ժ���� between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') and a.��Ժ����id=b.id

union all 
--ת���
select e.����Id,e.��ҳid,e.����,decode(a.����id,''$LesionID$'',a.����,b.����),decode(a.����id,''$LesionID$'',''3'',''4''),decode(a.����id,''$LesionID$'',d.����,c.����),a.��ֹʱ��
from ���˱䶯��¼ a,���˱䶯��¼ b,���ű� c,���ű� d,������ҳ e
where a.��ֹԭ��=3 and a.����id=b.����id and a.��ҳid=b.��ҳid and a.��ֹԭ��=b.��ʼԭ�� and a.��ֹʱ��=b.��ʼʱ�� and 
      a.��ֹʱ�� between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') and (a.����id=''$LesionID$'' or b.����id=''$LesionID$'') and a.����id=c.id and 
      b.����id=d.id and a.����id=e.����id and a.��ҳid=e.��ҳid 

union all 
--����������
Select distinct d.����Id,d.��ҳid,d.����,d.��Ժ����,decode(a.��Ŀ����,''����'',''6'',''7''),null,a.��¼ʱ��
From ���˻�����ϸ A, ���˻������� B, ���˻����ļ� C, ������ҳ D
Where a.��¼id = b.Id And b.�ļ�id = c.Id And c.����id = d.����id And c.��ҳid = d.��ҳid and d.��ǰ����id = ''$LesionID$'' and 
      a.��¼����=4 And a.��¼ʱ�� between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'')

union all 
--����
Select distinct d.����Id,d.��ҳid,d.����,d.��Ժ����,''5'',null,null
From ���˻�����ϸ A, ���˻������� B, ���˻����ļ� C, ������ҳ D
Where a.��¼id = b.Id And b.�ļ�id = c.Id And c.����id = d.����id And c.��ҳid = d.��ҳid and d.��ǰ����id = ''$LesionID$'' and 
      a.��Ŀ��� = 1 And Zl_To_Number(a.��¼����, 0) &gt; 37.5 And a.��¼ʱ��&gt;to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'')-3

union all 
--����
select distinct a.����Id,a.��ҳid,a.����,a.��Ժ����,''8'',c.����,b.��ʼִ��ʱ��
from ������ҳ a ,����ҽ����¼ b,���ű� c,������ĿĿ¼ d  
where a.����id=b.����id and a.��ҳid=b.��ҳid and a.��ǰ����id=''$LesionID$'' and b.ִ�п���id=c.id and 
      b.������Ŀid=d.id and d.���=''Z'' and d.��������=''11'' and b.����ʱ�� between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') and exists
      (select 1 from ����ҽ������ e where e.ҽ��id=b.id) 

union all      
--9������  10����Σ
select distinct a.����Id,a.��ҳid,a.����,a.��Ժ����,d.��������,c.����,b.��ʼִ��ʱ��
from ������ҳ a ,����ҽ����¼ b,���ű� c,������ĿĿ¼ d 
where a.����id=b.����id and a.��ҳid=b.��ҳid and a.��ǰ����id=''$LesionID$'' and b.ִ�п���id=c.id and
      b.������Ŀid=d.id and d.���=''Z'' and d.�������� in(''9'',''10'') and b.ҽ��״̬ in(3,8,9) and 
      (b.��ʼִ��ʱ�� between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') or (b.��ʼִ��ʱ��&lt;to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'') and 
      (nvl(b.ִ����ֹʱ��,to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'')+1/24/60/60)&gt;to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'') or nvl(b.ִ����ֹʱ��,to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'')+1/24/60/60) between to_date(''$StartTime$'',''yyyy-mm-dd hh24:mi:ss'')and to_date(''$EndTime$'',''yyyy-mm-dd hh24:mi:ss'')))) 
) a
</IN>');
begin
Insert into ϵͳ_������ (ID,����,����,˵��,���ģ��,����ģ��,�첽,����Դ,�Ƿ�����,����ʱ��,�޸��û�,�޸�ʱ��,�Ƿ񱾵ش洢,�Ƿ�ϵͳ����,���ն���ID,�Ƿ���ձ����) 
values ('48373493-109f-4af6-b9dd-64bf4166d345','���౨��',4,null,xmlStr,'<Out>{{Out}}</Out>',0,'ef090fc0-a178-42a7-81da-5f6d5021bf81',0,null,null,null,0,0,null,0);
end