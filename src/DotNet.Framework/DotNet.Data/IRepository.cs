// ===============================================================================
// DotNet.Platform ������� 2016 ��Ȩ����
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DotNet.Collections;
using DotNet.Utility;

namespace DotNet.Data
{
    /// <summary>
    /// ���ݴ洢���ӿ�
    /// </summary>
    /// <typeparam name="T">ʵ������</typeparam>
    public interface IRepository<T> where T : class, new()
    {
        /// <summary>
        /// ������ѯ����
        /// </summary>
        SQLQuery<T> SQL { get; }

        /// <summary>
        /// �Ƿ����ָ�������ļ�¼(��������)
        /// </summary>
        /// <param name="whereCondition">��ѯ�����ַ���(����where,����: name=@name and id=@id)</param>
        /// <param name="args">������������</param>
        /// <returns>�ҵ����������ļ�¼����true,���򷵻�false</returns>
        bool Exists(string whereCondition, object args = null);

        /// <summary>
        /// �Ƿ����ָ������ֵ�ļ�¼(����ֵ)
        /// </summary>
        /// <param name="primaryKey">����ֵ</param>
        /// <returns>���ڷ���true,���򷵻�false</returns>
        bool Exists(object primaryKey);

        /// <summary>
        /// �Ƿ����ָ�������ļ�¼(��ѯ���ʽ)
        /// </summary>
        /// <param name="expression">��ѯ���ʽ</param>
        /// <returns>���ڷ���true,���򷵻�false</returns>
        bool Exists(Expression<Func<T, bool>> expression);

        /// <summary>
        /// ��������(��Ҫд����к�ֵ����,֧���ֵ����ͺͶ�������,���������)
        /// </summary>
        /// <param name="args">��������(��Ҫд����к�ֵ����,֧���ֵ����ͺͶ�������,����������ֶ�)</param>
        /// <returns>������Ӱ�������</returns>
        int Insert(object args);

        /// <summary>
        /// ��������(ʵ�����,�������)
        /// </summary>
        /// <param name="entity">ʵ�����</param>
        /// <returns>�����������¼������ֵ</returns>
        object Insert(T entity);

        /// <summary>
        /// ��������(��Ҫд����к�ֵ����,֧���ֵ����ͺͶ�������)
        /// </summary>
        /// <param name="valueArgs">�������ݲ���(��Ҫ���µ��к�ֵ����,֧���ֵ����ͺͶ�������,����������ֶ�)</param>
        /// <param name="whereCondition">��ѯ�����ַ���(����where,����: name=@name and id=@id)</param>
        /// <param name="args">������������</param>
        /// <returns>������Ӱ�������</returns>
        int Update(object valueArgs, string whereCondition, object args = null);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="entity">ʵ�����</param>
        /// <param name="columns">ָ����Ҫ���µ���������,���Ϊ����������е���</param>
        /// <param name="whereCondition">��ѯ�����ַ���(����where,����: name=@name and id=@id)</param>
        /// <param name="args">������������</param>
        /// <returns>������Ӱ�������</returns>
        int Update(T entity, string[] columns, string whereCondition, object args = null);

        /// <summary>
        /// ����ʵ������
        /// </summary>
        /// <param name="entity">ʵ�����</param>
        /// <param name="columns">ָ����Ҫ���µ���������,���Ϊ����������е���</param>
        /// <returns>������Ӱ�������</returns>
        int Update(T entity, string[] columns);

        /// <summary>
        /// ����ʵ������
        /// </summary>
        /// <param name="entity">ʵ�����</param>
        /// <param name="columns">ָ����Ҫ���µ���������,���Ϊ����������е���</param>
        /// <returns>������Ӱ�������</returns>
        int Update(T entity, params Expression<Func<T, object>>[] columns);

        /// <summary>
        /// ����ʵ������(��ѯ���ʽ)
        /// </summary>
        /// <param name="entity">ʵ�����</param>
        /// <param name="expression">��ѯ���ʽ</param>
        /// <param name="columns">ָ����Ҫ���µ���������,���Ϊ����������е���</param>
        /// <returns>������Ӱ�������</returns>
        int Update(T entity, Expression<Func<T, bool>> expression, string[] columns);

        /// <summary>
        /// ����ʵ������(��ѯ���ʽ)
        /// </summary>
        /// <param name="entity">ʵ�����</param>
        /// <param name="expression">��ѯ���ʽ</param>
        /// <param name="columns">ָ����Ҫ���µ���������,���Ϊ����������е���</param>
        /// <returns>������Ӱ�������</returns>
        int Update(T entity, Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] columns);

        /// <summary>
        /// ��������(�����ⲿ��������)
        /// </summary>
        /// <param name="changedData">�ı������</param>
        int BatchUpdate(IEnumerable<PrimaryKeyValue> changedData);

        /// <summary>
        /// ɾ������(��������)
        /// </summary>
        /// <param name="whereCondition">��ѯ�����ַ���(����where,����: name=@name and id=@id)</param>
        /// <param name="args">��������</param>
        /// <returns>������Ӱ�������</returns>
        int Delete(string whereCondition, object args);

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="expression">��ѯ���ʽ</param>
        /// <returns>������Ӱ�������</returns>
        int Delete(Expression<Func<T, bool>> expression);

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="ids">��������</param>
        /// <returns>������Ӱ�������</returns>
        int Delete(Array ids);

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="id">��������</param>
        /// <returns>������Ӱ�������</returns>
        int Delete(int id);

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="id">�ַ�������</param>
        /// <returns>������Ӱ�������</returns>
        int Delete(string id);

        /// <summary>
        /// ɾ����������
        /// </summary>
        /// <returns>������Ӱ�������</returns>
        int DeleteAll();

        /// <summary>
        /// ��ȡʵ�����(����ֵ)
        /// </summary>
        /// <param name="id">����ֵ</param>
        /// <param name="columns">��ѯ������,���Ϊnull��ѯ������</param>
        /// <returns>����ʵ�����</returns>
        T Get(object id, string[] columns);

        /// <summary>
        /// ��ȡʵ�����(����ֵ)
        /// </summary>
        /// <param name="id">����ֵ</param>
        /// <param name="columns">��ѯ������,���Ϊnull��ѯ������</param>
        /// <returns>����ʵ�����</returns>
        T Get(object id, params Expression<Func<T, object>>[] columns);

        /// <summary>
        /// ��ȡʵ�����(��ѯ���ʽ)
        /// </summary>
        /// <param name="linq">��ѯ���ʽ</param>
        /// <returns>����ʵ�����</returns>
        T Get(SQLQuery<T> linq);

        /// <summary>
        /// ��ѯ����(��ѯ���ʽ)
        /// </summary>
        /// <param name="linq">��ѯ���ʽ</param>
        /// <returns>���ؼ�¼����</returns>
        IEnumerable<T> Query(SQLQuery<T> linq);

        /// <summary>
        /// ��ѯ����(Ĭ���������)
        /// </summary>
        /// <param name="expression">��ѯ���ʽ</param>
        /// <param name="columns">��ѯ��</param>
        /// <returns>���ؼ�¼����</returns>
        IEnumerable<T> Query(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] columns);

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <returns>���ؼ�¼����</returns>
        IEnumerable<T> Query();

        /// <summary>
        /// ��ѯ����(��ѯ���ʽ)
        /// </summary>
        /// <param name="linq">��ѯ���ʽ</param>
        /// <returns>���ؼ�¼����</returns>
        List<T> Fetch(SQLQuery<T> linq);

        /// <summary>
        /// ��ѯ����(Ĭ���������)
        /// </summary>
        /// <param name="expression">��ѯ���ʽ</param>
        /// <param name="columns">��ѯ��</param>
        /// <returns>���ؼ�¼����</returns>
        List<T> Fetch(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] columns);

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <returns>���ؼ�¼����</returns>
        List<T> Fetch();

        /// <summary>
        /// ��ѯ��ҳ����
        /// </summary>
        /// <param name="linq">��ѯ���ʽ</param>
        /// <returns>���ط�ҳ�б�</returns>
        PageList<T> Page(SQLQuery<T> linq);
    }
}