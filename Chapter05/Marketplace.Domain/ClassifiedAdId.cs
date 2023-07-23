using System;

namespace Marketplace.Domain
{
    public class ClassifiedAdId : IEquatable<ClassifiedAdId>
    {
        private Guid Value { get; }

        public ClassifiedAdId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentNullException(nameof(value), "Classified Ad id cannot be empty");

            Value = value;
        }

        /// <summary>
        /// 定義將 ClassifiedAdId 物件轉換成 Guid 型別的操作。
        /// ClassifiedAdId 是一個類別，其中包含了一個 Value 屬性，類型為 Guid。
        /// 使用這個轉換運算子將 ClassifiedAdId 轉換成 Guid 時，是取出 ClassifiedAdId 的 Value 屬性，並將其轉換成 Guid 型別。
        /// 可以直接將 ClassifiedAdId 物件當作 Guid 使用，而不需要明確地調用 self.Value 來取得其中的 Guid 值。
        /// </summary>
        /// <param name="self">ClassifiedAdId</param>
        public static implicit operator Guid(ClassifiedAdId self) => self.Value;

        /// <summary>
        /// 定義將 string 型別轉換成 ClassifiedAdId 的操作。
        /// 在這個轉換運算子中，接受一個 string 參數 value，
        /// 然後使用 Guid.Parse 方法將這個 string 值解析成一個 Guid 值。
        /// 使用這個 Guid 值來建立一個新的 ClassifiedAdId 物件，並將其作為結果返回。
        /// 就可以直接將一個 string 值當作 ClassifiedAdId 物件使用，而不需要手動進行 Guid 的解析和 ClassifiedAdId 物件的建立。
        /// </summary>
        /// <param name="value">string</param>
        public static implicit operator ClassifiedAdId(string value)
            => new ClassifiedAdId(Guid.Parse(value));

        public override string ToString() => Value.ToString();

        public bool Equals(ClassifiedAdId other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((ClassifiedAdId)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}