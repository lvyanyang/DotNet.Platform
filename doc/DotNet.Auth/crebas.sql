/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2016/7/3 22:34:43                            */
/*==============================================================*/


if exists (select 1
            from  sysindexes
           where  id    = object_id('Departments')
            and   name  = 'UK_Departments_Name'
            and   indid > 0
            and   indid < 255)
   drop index Departments.UK_Departments_Name
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Departments')
            and   type = 'U')
   drop table Departments
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('DicDetails')
            and   name  = 'UK_DicDetails_Name'
            and   indid > 0
            and   indid < 255)
   drop index DicDetails.UK_DicDetails_Name
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DicDetails')
            and   type = 'U')
   drop table DicDetails
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Dics')
            and   name  = 'UK_Dics_Code'
            and   indid > 0
            and   indid < 255)
   drop index Dics.UK_Dics_Code
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Dics')
            and   type = 'U')
   drop table Dics
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Logs')
            and   type = 'U')
   drop table Logs
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Menus')
            and   name  = 'UK_Menus_Code'
            and   indid > 0
            and   indid < 255)
   drop index Menus.UK_Menus_Code
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Menus')
            and   type = 'U')
   drop table Menus
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Roles')
            and   name  = 'UK_Roles_Name'
            and   indid > 0
            and   indid < 255)
   drop index Roles.UK_Roles_Name
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Roles')
            and   type = 'U')
   drop table Roles
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Seqs')
            and   name  = 'UK_Seqs_Name'
            and   indid > 0
            and   indid < 255)
   drop index Seqs.UK_Seqs_Name
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Seqs')
            and   type = 'U')
   drop table Seqs
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Users')
            and   name  = 'UK_Users_Account'
            and   indid > 0
            and   indid < 255)
   drop index Users.UK_Users_Account
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Users')
            and   type = 'U')
   drop table Users
go

/*==============================================================*/
/* Table: Departments                                           */
/*==============================================================*/
create table Departments (
   Id                   nvarchar(50)         not null,
   ParentId             nvarchar(50)         not null,
   Name                 nvarchar(100)        not null,
   Spell                nvarchar(100)        null,
   DepartmentLeaderId   nvarchar(50)         null,
   DepartmentLeaderName nvarchar(100)        null,
   ChargeLeaderId       nvarchar(50)         null,
   ChargeLeaderName     nvarchar(100)        null,
   MainLeaderId         nvarchar(50)         null,
   MainLeaderName       nvarchar(100)        null,
   SortPath             nvarchar(1000)       not null,
   CreateUserId         int                  null,
   CreateUserName       nvarchar(100)        null,
   CreateDateTime       datetime             null,
   ModifyUserId         int                  null,
   ModifyUserName       nvarchar(100)        null,
   ModifyDateTime       datetime             null,
   Note                 nvarchar(1000)       null,
   constraint PK_Departments_Id primary key nonclustered (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Departments') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Departments' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '系统部门', 
   'user', @CurrentUser, 'table', 'Departments'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ParentId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'ParentId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '父Id',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'ParentId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Name')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'Name'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '名称',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'Name'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Spell')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'Spell'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '简拼',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'Spell'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'DepartmentLeaderId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'DepartmentLeaderId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '部门领导Id',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'DepartmentLeaderId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'DepartmentLeaderName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'DepartmentLeaderName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '部门领导名称',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'DepartmentLeaderName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ChargeLeaderId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'ChargeLeaderId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '分管领导Id',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'ChargeLeaderId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ChargeLeaderName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'ChargeLeaderName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '分管领导名称',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'ChargeLeaderName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'MainLeaderId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'MainLeaderId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '主管领导Id',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'MainLeaderId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'MainLeaderName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'MainLeaderName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '主管领导名称',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'MainLeaderName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'SortPath')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'SortPath'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '排序路径',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'SortPath'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'CreateUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建用户Id',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'CreateUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'CreateUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建用户姓名',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'CreateUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'CreateDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'CreateDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'ModifyUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改用户Id',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'ModifyUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'ModifyUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改用户姓名',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'ModifyUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'ModifyDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改时间',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'ModifyDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Departments')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Note')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Departments', 'column', 'Note'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '备注',
   'user', @CurrentUser, 'table', 'Departments', 'column', 'Note'
go

/*==============================================================*/
/* Index: UK_Departments_Name                                   */
/*==============================================================*/
create unique index UK_Departments_Name on Departments (
ParentId ASC,
Name ASC
)
go

/*==============================================================*/
/* Table: DicDetails                                            */
/*==============================================================*/
create table DicDetails (
   Id                   nvarchar(50)         not null,
   DicId                nvarchar(50)         not null,
   Name                 nvarchar(100)        not null,
   Spell                nvarchar(100)        null,
   Value                nvarchar(100)        null,
   IsEnabled            bit                  not null default 1,
   RowIndex             int                  not null,
   CreateUserId         int                  null,
   CreateUserName       nvarchar(100)        null,
   CreateDateTime       datetime             null,
   ModifyUserId         int                  null,
   ModifyUserName       nvarchar(100)        null,
   ModifyDateTime       datetime             null,
   Note                 nvarchar(1000)       null,
   constraint PK_DicDetails_Id primary key nonclustered (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('DicDetails') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'DicDetails' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '系统字典明细', 
   'user', @CurrentUser, 'table', 'DicDetails'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'DicId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'DicId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '字典Id',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'DicId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Name')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'Name'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '项名称',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'Name'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Spell')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'Spell'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '项简拼',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'Spell'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Value')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'Value'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '项值',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'Value'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsEnabled')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'IsEnabled'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '启用',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'IsEnabled'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'RowIndex')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'RowIndex'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '序号',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'RowIndex'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'CreateUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建用户Id',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'CreateUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'CreateUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建用户姓名',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'CreateUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'CreateDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'CreateDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'ModifyUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改用户Id',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'ModifyUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'ModifyUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改用户姓名',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'ModifyUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'ModifyDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改时间',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'ModifyDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('DicDetails')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Note')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'Note'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '备注',
   'user', @CurrentUser, 'table', 'DicDetails', 'column', 'Note'
go

/*==============================================================*/
/* Index: UK_DicDetails_Name                                    */
/*==============================================================*/
create unique index UK_DicDetails_Name on DicDetails (
DicId ASC,
Name ASC
)
go

/*==============================================================*/
/* Table: Dics                                                  */
/*==============================================================*/
create table Dics (
   Id                   nvarchar(50)         not null,
   ParentId             nvarchar(50)         not null default '0',
   Code                 nvarchar(100)        not null,
   Name                 nvarchar(100)        not null,
   Spell                nvarchar(100)        null,
   CreateUserId         int                  null,
   CreateUserName       nvarchar(100)        null,
   CreateDateTime       datetime             null,
   ModifyUserId         int                  null,
   ModifyUserName       nvarchar(100)        null,
   ModifyDateTime       datetime             null,
   SortPath             nvarchar(1000)       not null,
   Note                 nvarchar(1000)       null,
   constraint PK_Dics_Id primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Dics') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Dics' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '系统字典', 
   'user', @CurrentUser, 'table', 'Dics'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ParentId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'ParentId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '父Id',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'ParentId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Code')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'Code'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '编码',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'Code'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Name')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'Name'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '名称',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'Name'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Spell')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'Spell'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '简拼',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'Spell'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'CreateUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建用户Id',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'CreateUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'CreateUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建用户姓名',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'CreateUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'CreateDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'CreateDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'ModifyUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改用户Id',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'ModifyUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'ModifyUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改用户姓名',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'ModifyUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'ModifyDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改时间',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'ModifyDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'SortPath')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'SortPath'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '排序路径',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'SortPath'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Dics')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Note')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Dics', 'column', 'Note'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '备注',
   'user', @CurrentUser, 'table', 'Dics', 'column', 'Note'
go

/*==============================================================*/
/* Index: UK_Dics_Code                                          */
/*==============================================================*/
create unique index UK_Dics_Code on Dics (
Code ASC
)
go

/*==============================================================*/
/* Table: Logs                                                  */
/*==============================================================*/
create table Logs (
   Id                   nvarchar(50)         not null,
   Message              nvarchar(Max)        null,
   IPAddress            nvarchar(50)         null,
   CreateUserId         nvarchar(50)         null,
   CreateUserName       nvarchar(100)        null,
   CreateDateTime       datetime             not null,
   constraint PK_Logs_Id primary key nonclustered (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Logs') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Logs' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '系统日志', 
   'user', @CurrentUser, 'table', 'Logs'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Logs')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Logs', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'Logs', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Logs')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Message')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Logs', 'column', 'Message'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '内容',
   'user', @CurrentUser, 'table', 'Logs', 'column', 'Message'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Logs')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IPAddress')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Logs', 'column', 'IPAddress'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'IP地址',
   'user', @CurrentUser, 'table', 'Logs', 'column', 'IPAddress'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Logs')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Logs', 'column', 'CreateUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '操作用户主键',
   'user', @CurrentUser, 'table', 'Logs', 'column', 'CreateUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Logs')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Logs', 'column', 'CreateUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '操作用户姓名',
   'user', @CurrentUser, 'table', 'Logs', 'column', 'CreateUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Logs')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Logs', 'column', 'CreateDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '操作时间',
   'user', @CurrentUser, 'table', 'Logs', 'column', 'CreateDateTime'
go

/*==============================================================*/
/* Table: Menus                                                 */
/*==============================================================*/
create table Menus (
   Id                   nvarchar(50)         not null,
   ParentId             nvarchar(50)         not null,
   Code                 nvarchar(100)        not null,
   Name                 nvarchar(100)        not null,
   Spell                nvarchar(100)        null,
   Url                  nvarchar(1000)       null,
   IconCls              nvarchar(100)        null,
   IsExpand             bit                  not null default 0,
   IsPublic             bit                  not null default 0,
   IsEnabled            bit                  not null default 1,
   SortPath             nvarchar(1000)       not null,
   CreateUserId         int                  null,
   CreateUserName       nvarchar(100)        null,
   CreateDateTime       datetime             null,
   ModifyUserId         int                  null,
   ModifyUserName       nvarchar(100)        null,
   ModifyDateTime       datetime             null,
   Note                 nvarchar(1000)       null,
   constraint PK_Menus_Id primary key nonclustered (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Menus') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Menus' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '系统菜单', 
   'user', @CurrentUser, 'table', 'Menus'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ParentId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'ParentId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '父Id',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'ParentId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Code')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'Code'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '编码',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'Code'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Name')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'Name'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '名称',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'Name'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Spell')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'Spell'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '简拼',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'Spell'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Url')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'Url'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '导航地址',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'Url'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IconCls')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'IconCls'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '图标样式',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'IconCls'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsExpand')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'IsExpand'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否展开',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'IsExpand'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsPublic')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'IsPublic'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否公开',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'IsPublic'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsEnabled')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'IsEnabled'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否启用',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'IsEnabled'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'SortPath')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'SortPath'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '排序路径',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'SortPath'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'CreateUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建用户Id',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'CreateUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'CreateUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建用户姓名',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'CreateUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'CreateDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'CreateDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'ModifyUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改用户Id',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'ModifyUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'ModifyUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改用户姓名',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'ModifyUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'ModifyDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改时间',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'ModifyDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menus')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Note')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menus', 'column', 'Note'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '备注',
   'user', @CurrentUser, 'table', 'Menus', 'column', 'Note'
go

/*==============================================================*/
/* Index: UK_Menus_Code                                         */
/*==============================================================*/
create unique index UK_Menus_Code on Menus (
Code ASC
)
go

/*==============================================================*/
/* Table: Roles                                                 */
/*==============================================================*/
create table Roles (
   Id                   nvarchar(50)         not null,
   Name                 nvarchar(100)        not null,
   Spell                nvarchar(100)        null,
   CategoryId           nvarchar(100)        not null,
   CategoryName         nvarchar(100)        not null,
   IsEnabled            bit                  not null default 1,
   RowIndex             int                  not null,
   CreateUserId         int                  null,
   CreateUserName       nvarchar(100)        null,
   CreateDateTime       datetime             null,
   ModifyUserId         int                  null,
   ModifyUserName       nvarchar(100)        null,
   ModifyDateTime       datetime             null,
   Note                 nvarchar(1000)       null,
   constraint PK_Roles_Id primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Roles') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Roles' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '系统角色', 
   'user', @CurrentUser, 'table', 'Roles'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Name')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'Name'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '名称',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'Name'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Spell')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'Spell'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '简拼',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'Spell'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CategoryId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'CategoryId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '分类Id',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'CategoryId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CategoryName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'CategoryName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '分类名称',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'CategoryName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsEnabled')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'IsEnabled'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '启用',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'IsEnabled'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'RowIndex')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'RowIndex'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '序号',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'RowIndex'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'CreateUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建用户Id',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'CreateUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'CreateUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建用户姓名',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'CreateUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'CreateDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'CreateDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'ModifyUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改用户Id',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'ModifyUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'ModifyUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改用户姓名',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'ModifyUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'ModifyDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改时间',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'ModifyDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Roles')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Note')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Roles', 'column', 'Note'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '备注',
   'user', @CurrentUser, 'table', 'Roles', 'column', 'Note'
go

/*==============================================================*/
/* Index: UK_Roles_Name                                         */
/*==============================================================*/
create unique index UK_Roles_Name on Roles (
Name ASC
)
go

/*==============================================================*/
/* Table: Seqs                                                  */
/*==============================================================*/
create table Seqs (
   Id                   nvarchar(50)         not null,
   Name                 nvarchar(100)        not null,
   Value                int                  not null default 1,
   Step                 int                  not null default 1,
   Note                 nvarchar(1000)       null,
   constraint PK_Seqs_Id primary key nonclustered (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Seqs') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Seqs' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '系统序列', 
   'user', @CurrentUser, 'table', 'Seqs'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Seqs')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Seqs', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'Seqs', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Seqs')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Name')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Seqs', 'column', 'Name'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '名称',
   'user', @CurrentUser, 'table', 'Seqs', 'column', 'Name'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Seqs')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Value')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Seqs', 'column', 'Value'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '值',
   'user', @CurrentUser, 'table', 'Seqs', 'column', 'Value'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Seqs')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Step')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Seqs', 'column', 'Step'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '步长',
   'user', @CurrentUser, 'table', 'Seqs', 'column', 'Step'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Seqs')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Note')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Seqs', 'column', 'Note'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '备注',
   'user', @CurrentUser, 'table', 'Seqs', 'column', 'Note'
go

/*==============================================================*/
/* Index: UK_Seqs_Name                                          */
/*==============================================================*/
create unique clustered index UK_Seqs_Name on Seqs (
Name ASC
)
go

/*==============================================================*/
/* Table: Users                                                 */
/*==============================================================*/
create table Users (
   Id                   nvarchar(50)         not null,
   Account              nvarchar(100)        not null,
   Password             nvarchar(500)        not null,
   Name                 nvarchar(100)        not null,
   Spell                nvarchar(100)        null,
   DepartmentId         nvarchar(50)         null,
   DepartmentName       nvarchar(100)        null,
   AllowStartDateTime   datetime             null,
   AllowEndDateTime     datetime             null,
   FirstVisitDateTime   datetime             null,
   LastVisitDateTime    datetime             null,
   LoginCount           int                  not null default 0,
   IsAdmin              bit                  not null default 0,
   IsEnabled            bit                  not null default 1,
   IsAudit              bit                  not null default 1,
   RowIndex             int                  not null,
   Email                nvarchar(100)        null,
   HintQuestion         nvarchar(100)        null,
   HintAnswer           nvarchar(100)        null,
   DefaultRoleId        nvarchar(50)         null,
   DefaultRoleName      nvarchar(100)        null,
   CreateUserId         int                  null,
   CreateUserName       nvarchar(100)        null,
   CreateDateTime       datetime             null,
   ModifyUserId         int                  null,
   ModifyUserName       nvarchar(100)        null,
   ModifyDateTime       datetime             null,
   Note                 nvarchar(1000)       null,
   constraint PK_Users_Id primary key nonclustered (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Users') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Users' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '系统用户', 
   'user', @CurrentUser, 'table', 'Users'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Id',
   'user', @CurrentUser, 'table', 'Users', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Account')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'Account'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '帐号',
   'user', @CurrentUser, 'table', 'Users', 'column', 'Account'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Password')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'Password'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '密码',
   'user', @CurrentUser, 'table', 'Users', 'column', 'Password'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Name')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'Name'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '姓名',
   'user', @CurrentUser, 'table', 'Users', 'column', 'Name'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Spell')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'Spell'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '简拼',
   'user', @CurrentUser, 'table', 'Users', 'column', 'Spell'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'DepartmentId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'DepartmentId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '部门Id',
   'user', @CurrentUser, 'table', 'Users', 'column', 'DepartmentId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'DepartmentName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'DepartmentName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '部门名称',
   'user', @CurrentUser, 'table', 'Users', 'column', 'DepartmentName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'AllowStartDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'AllowStartDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '允许开始时间',
   'user', @CurrentUser, 'table', 'Users', 'column', 'AllowStartDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'AllowEndDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'AllowEndDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '允许结束时间',
   'user', @CurrentUser, 'table', 'Users', 'column', 'AllowEndDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'FirstVisitDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'FirstVisitDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '第一次登录时间',
   'user', @CurrentUser, 'table', 'Users', 'column', 'FirstVisitDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'LastVisitDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'LastVisitDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '最后一次登录时间',
   'user', @CurrentUser, 'table', 'Users', 'column', 'LastVisitDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'LoginCount')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'LoginCount'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '登录次数',
   'user', @CurrentUser, 'table', 'Users', 'column', 'LoginCount'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsAdmin')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'IsAdmin'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否管理员',
   'user', @CurrentUser, 'table', 'Users', 'column', 'IsAdmin'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsEnabled')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'IsEnabled'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否启用',
   'user', @CurrentUser, 'table', 'Users', 'column', 'IsEnabled'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsAudit')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'IsAudit'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否审核',
   'user', @CurrentUser, 'table', 'Users', 'column', 'IsAudit'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'RowIndex')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'RowIndex'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '序号',
   'user', @CurrentUser, 'table', 'Users', 'column', 'RowIndex'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Email')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'Email'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '电子邮件',
   'user', @CurrentUser, 'table', 'Users', 'column', 'Email'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'HintQuestion')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'HintQuestion'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '密码提示问题',
   'user', @CurrentUser, 'table', 'Users', 'column', 'HintQuestion'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'HintAnswer')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'HintAnswer'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '密码提示答案',
   'user', @CurrentUser, 'table', 'Users', 'column', 'HintAnswer'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'DefaultRoleId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'DefaultRoleId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '默认角色主键',
   'user', @CurrentUser, 'table', 'Users', 'column', 'DefaultRoleId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'DefaultRoleName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'DefaultRoleName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '默认角色名称',
   'user', @CurrentUser, 'table', 'Users', 'column', 'DefaultRoleName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'CreateUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建用户Id',
   'user', @CurrentUser, 'table', 'Users', 'column', 'CreateUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'CreateUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建用户姓名',
   'user', @CurrentUser, 'table', 'Users', 'column', 'CreateUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'CreateDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'Users', 'column', 'CreateDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyUserId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'ModifyUserId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改用户Id',
   'user', @CurrentUser, 'table', 'Users', 'column', 'ModifyUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'ModifyUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改用户姓名',
   'user', @CurrentUser, 'table', 'Users', 'column', 'ModifyUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ModifyDateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'ModifyDateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改时间',
   'user', @CurrentUser, 'table', 'Users', 'column', 'ModifyDateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Note')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Users', 'column', 'Note'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '备注',
   'user', @CurrentUser, 'table', 'Users', 'column', 'Note'
go

/*==============================================================*/
/* Index: UK_Users_Account                                      */
/*==============================================================*/
create unique index UK_Users_Account on Users (
Account ASC
)
go

