using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdText : Value<ClassifiedAdText>
    {
        public string Value { get; }

        internal ClassifiedAdText(string text) => Value = text;
        
        public static ClassifiedAdText FromString(string text) =>
            new ClassifiedAdText(text);

        /// <summary>
        /// 定義了將 ClassifiedAdText 物件轉換成 string 型別的操作。
        /// 使用這個轉換運算子將 ClassifiedAdText 物件轉換成 string 時，是取出了 ClassifiedAdText 的 Value 屬性，
        /// 並將其轉換成 string 型別。直接將 ClassifiedAdText 物件當作 string 使用，而不需要明確地調用 text.Value 來取得其中的 string 值。
        /// </summary>
        /// <param name="text">ClassifiedAdText</param>
        public static implicit operator string(ClassifiedAdText text) =>
            text.Value;
    }
}