using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Framework
{
    /// <summary>
    /// 這是一個泛型抽象類別，其中 TId 是用來表示實體的唯一識別（Identifier）的型別。
    /// 泛型參數 TId 限制為實作了 IEquatable<TId> 接口的型別，這表示 TId 型別可以進行相等性比較。
    /// 提供了處理領域事件和狀態驗證的機制，同時也使得外部程式可以查看實體狀態的變化。
    /// 子類別可以根據需要來實作 When 和 EnsureValidState 方法，並在應用領域事件時進行狀態的更新與驗證。
    /// </summary>
    /// <typeparam name="TId">實體</typeparam>
    public abstract class Entity<TId> where TId : IEquatable<TId>
    {
        /// <summary>
        /// 用於存儲已應用到實體上的事件。
        /// 可以理解為對於實體狀態的改變或更新的描述。
        /// </summary>
        private readonly List<object> _events;

        protected Entity() => _events = new List<object>();
        /// <summary>
        /// 用於將事件應用到實體上。
        /// Apply 方法接受一個事件對象作為參數，
        /// 然後調用 When 方法來處理該事件，
        /// 接著呼叫 EnsureValidState 方法來確保實體的狀態是有效的，最後將事件加入到 _events 列表中。
        /// </summary>
        /// <param name="event">事件對象</param>
        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _events.Add(@event);
        }
        /// <summary>
        /// 用於根據接收的事件對象來處理相應的事件邏輯。
        /// 每當 Apply 方法被呼叫時，它都會調用子類別的 When 方法。
        /// </summary>
        /// <param name="event"></param>
        protected abstract void When(object @event);

        /// <summary>
        /// 用於取得已經應用到實體上的事件列表。這使得外部程式可以查看實體狀態的變化。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetChanges() => _events.AsEnumerable();

        public void ClearChanges() => _events.Clear();

        /// <summary>
        /// 用於確保實體的狀態是有效的。
        /// 在 Apply 方法中被呼叫，以確保每個事件應用後的實體狀態符合領域邏輯和限制。
        /// </summary>
        protected abstract void EnsureValidState();
    }
}