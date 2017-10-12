using EFCore2.Models.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFCore2.Models
{
    //Step 3
    //public class JoinCollectionFacade<T, TJoin> : ICollection<T>
    //{
    //    private readonly ICollection<TJoin> _collection;
    //    private readonly Func<TJoin, T> _selector;
    //    private readonly Func<T, TJoin> _creator;

    //    public JoinCollectionFacade(
    //        ICollection<TJoin> collection,
    //        Func<TJoin, T> selector,
    //        Func<T, TJoin> creator)
    //    {
    //        _collection = collection;
    //        _selector = selector;
    //        _creator = creator;
    //    }

    //    public IEnumerator<T> GetEnumerator()
    //        => _collection.Select(e => _selector(e)).GetEnumerator();

    //    IEnumerator IEnumerable.GetEnumerator()
    //        => GetEnumerator();

    //    public void Add(T item)
    //        => _collection.Add(_creator(item));

    //    public void Clear()
    //        => _collection.Clear();

    //    public bool Contains(T item)
    //        => _collection.Any(e => Equals(_selector(e), item));

    //    public void CopyTo(T[] array, int arrayIndex)
    //        => this.ToList().CopyTo(array, arrayIndex);

    //    public bool Remove(T item)
    //        => _collection.Remove(
    //            _collection.FirstOrDefault(e => Equals(_selector(e), item)));

    //    public int Count
    //        => _collection.Count;

    //    public bool IsReadOnly
    //        => _collection.IsReadOnly;
    //}

    //Step 4
    public class JoinCollectionFacade<TEntity, TOtherEntity, TJoinEntity>
    : ICollection<TEntity>
    where TJoinEntity : IJoinEntity<TEntity>, IJoinEntity<TOtherEntity>, new()
    {
        private readonly TOtherEntity _ownerEntity;
        private readonly ICollection<TJoinEntity> _collection;

        public JoinCollectionFacade(
            TOtherEntity ownerEntity,
            ICollection<TJoinEntity> collection)
        {
            _ownerEntity = ownerEntity;
            _collection = collection;
        }

        public IEnumerator<TEntity> GetEnumerator()
            => _collection.Select(e => ((IJoinEntity<TEntity>)e).Navigation).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public void Add(TEntity item)
        {
            var entity = new TJoinEntity();
            ((IJoinEntity<TEntity>)entity).Navigation = item;
            ((IJoinEntity<TOtherEntity>)entity).Navigation = _ownerEntity;
            _collection.Add(entity);
        }

        public void Clear()
            => _collection.Clear();

        public bool Contains(TEntity item)
            => _collection.Any(e => Equals(item, e));

        public void CopyTo(TEntity[] array, int arrayIndex)
            => this.ToList().CopyTo(array, arrayIndex);

        public bool Remove(TEntity item)
            => _collection.Remove(
                _collection.FirstOrDefault(e => Equals(item, e)));

        public int Count
            => _collection.Count;

        public bool IsReadOnly
            => _collection.IsReadOnly;

        private static bool Equals(TEntity item, TJoinEntity e)
            => Equals(((IJoinEntity<TEntity>)e).Navigation, item);
    }
}
