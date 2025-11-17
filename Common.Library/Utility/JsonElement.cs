using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utility
{
    /// <summary>Enumeration of Json element types.</summary>
    internal enum JsonElementType { Null, String, Number, Boolean, Object, Array }
    internal abstract class JsonElement
    {
        protected object Value { get; set; }
        /// <summary>Json element types.</summary>
        public JsonElementType JEleType { get; set; } = JsonElementType.Null;

        public T ToObject<T>() => (T)Convert.ChangeType(Value, typeof(T));

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>Parse and convert a string to the Json element type.</summary>
        /// <param name="jsonStr">The json string that needs to be parsed and converted.</param>
        /// <returns>Return the json element that has been successfully parsed and converted.</returns>
        public static JsonElement Parse(string jsonStr)
        {
            string text = jsonStr.Trim();
            if ((!text.StartsWith("{") || !text.EndsWith("}")) && (!text.StartsWith("[") || !text.EndsWith("]")))
                throw new JsonParseException("The provided JSON string is invalid. The starting position of the JSON string does not contain the \"{\" or \"[\" symbol or the ending position does not contain the \"}\" or \"]\" symbol.");
            var index = 0;
            // Parse json array
            if (text.StartsWith("["))
                return ReadJsonArray(text, ref index);
            // Parse json object
            return ReadJsonObject(text, ref index);
        }

        /// <summary>Parse and convert a string to the Json object element type.</summary>
        /// <param name="jsonStr">The json string that needs to be parsed and converted.</param>
        /// <returns>Return the json object element that has been successfully parsed and converted.</returns>
        public static JsonObject ParseJsonObject(string jsonStr)
        {
            string text = jsonStr.Trim();
            if (!text.StartsWith("{") || !text.EndsWith("}"))
                throw new JsonParseException("The provided JSON string is invalid. The starting position of the JSON object string does not contain the \"{\" symbol or the ending position does not contain the \"}\" symbol.");
            var index = 0;
            return ReadJsonObject(jsonStr, ref index);
        }
        private static JsonObject ReadJsonObject(string jsonStr, ref int index)
        {
            JsonObject jsonObj = new JsonObject();
            do
            {
                ReadToNonBlankIndex(jsonStr, ref index);
                // Read the element name
                string name = ReadJsonString(jsonStr, ref index);
                if (jsonObj.ContainsKey(name))
                    throw new JsonParseException($"Duplicate key values appear near the index {index}");
                // Read the element value
                jsonObj.Add(name, ReadJsonElement(jsonStr, ref index));
                ReadToNonBlankIndex(jsonStr, ref index);
                // Read the next character to determine whether to read the next key value or end
                var c = jsonStr[index++];
                if (c == ',') continue;
                if (c == '}') break; // Match the end identifier

            } while (true);
            return jsonObj;
        }

        /// <summary>Parse and convert a string to the Json array type.</summary>
        /// <param name="jsonStr">The json string that needs to be parsed and converted.</param>
        /// <returns>Return the json array that has been successfully parsed and converted.</returns>
        public static JsonArray ParseJsonArray(string jsonStr)
        {
            string text = jsonStr.Trim();
            if (!text.StartsWith("[") || !text.EndsWith("]"))
                throw new JsonParseException("The provided JSON string is invalid. The starting position of the JSON Array string does not contain the \"[\" symbol or the ending position does not contain the \"]\" symbol.");
            var index = 0;
            return ReadJsonArray(text, ref index);
        }
        private static JsonArray ReadJsonArray(string jsonStr, ref int index)
        {
            JsonArray jsonArr = new JsonArray();
            ReadToNonBlankIndex(jsonStr, ref index);
            var c = jsonStr[index];
            if (c == '[') index++; // Skip the leading identifier
            do
            {
                ReadToNonBlankIndex(jsonStr, ref index);
                // Read the array element
                jsonArr.Add(ReadJsonElement(jsonStr, ref index));
                ReadToNonBlankIndex(jsonStr, ref index);
                // Read the next character to determine whether to read the next element or end
                c = jsonStr[index++];
                if (c == ',') continue;
                if (c == ']') break; // Match the end identifier

            } while (true);
            return jsonArr;
        }

        /// <summary>Read a string element.</summary>
        /// <param name="text">The string to be read.</param>
        /// <param name="index">The starting position for reading.</param>
        /// <returns>Return a string element that was successfully read.</returns>
        private static string ReadJsonString(string text, ref int index)
        {
            // Find the starting position of the double quotation marks for characters.
            index = text.IndexOf('"', index) + 1;
            StringBuilder value = new StringBuilder();
            while (index < text.Length)
            {
                var c = text[index++];
                // Check if it is an escape character
                if (c == '\\')
                {
                    value.Append('\\');
                    c = text[index++];
                    value.Append(c);
                    if (c == 'u')
                    {
                        // Check if it is a hexadecimal character.
                        for (int i = 0; i < 4; i++)
                        {
                            c = text[index++];
                            if (IsHex(c)) { value.Append(c); }
                            else { throw new JsonParseException("Not a valid Unicode character!"); }
                        }
                    }
                }
                else if (c == '\r' || c == '\n') { throw new JsonParseException("Line breaks are not allowed in JSON string content!"); }
                else if (c == '"') { break; }
                else { value.Append(c); }
            }
            return value.ToString();
        }
        /// <summary>Check if it is a hexadecimal character.</summary>
        private static bool IsHex(char c) => c >= '0' && c <= '9' || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F';
        /// <summary>Read the index of the first non-null character from the specified starting position.</summary>
        /// <param name="text">The string to be read.</param>
        /// <param name="index">The starting position for reading.</param>
        private static void ReadToNonBlankIndex(string text, ref int index) { while (index < text.Length && char.IsWhiteSpace(text[index])) index++; }

        /// <summary>Read a json element.</summary>
        /// <param name="text">The string to be read.</param>
        /// <param name="index">The starting position for reading.</param>
        /// <returns>Return a json element that was successfully read.</returns>
        private static JsonElement ReadJsonElement(string text, ref int index)
        {
            do
            {
                ReadToNonBlankIndex(text, ref index);
                switch (text[index++])
                {
                    case '[': return ReadJsonArray(text, ref index);
                    case '{': return ReadJsonObject(text, ref index);
                    case '"': index--; return new JsonString(ReadJsonString(text, ref index));
                    case 'T':
                    case 't': return ReadJsonBool(text, true, ref index);
                    case 'F':
                    case 'f': return ReadJsonBool(text, false, ref index);
                    case 'n': return ReadJsonNull(text, ref index);
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9': return ReadJsonNumber(text, ref index);
                    case '：':
                    default: continue;
                }
            } while (true);
        }

        /// <summary>Read a json element of Boolean type.</summary>
        /// <param name="text">The string to be read.</param>
        /// <param name="isTrue">Is the predicted Boolean type true or false.</param>
        /// <param name="index">The starting position for reading.</param>
        /// <returns>Return a successfully read Boolean type json element.</returns>
        private static JsonBoolean ReadJsonBool(string text, bool isTrue, ref int index)
        {
            index--;
            string value = isTrue ? text.Substring(index, 4) : text.Substring(index, 5);
            if (value != "true" && value != "false")
                throw new JsonParseException($"Unidentifiable Boolean type: {value} , Index position {index}.");
            index = index + value.Length;
            return new JsonBoolean(value);
        }

        /// <summary>Read a json element of null type.</summary>
        /// <param name="text">The string to be read.</param>
        /// <param name="index">The starting position for reading.</param>
        /// <returns>Return a successfully read null type json element.</returns>
        private static JsonNull ReadJsonNull(string text, ref int index)
        {
            index--;
            string value = text.Substring(index, 4);
            if (value != "null")
                throw new JsonParseException($"Unidentifiable null type: {value} , Index position {index}.");
            index = index + value.Length;
            return new JsonNull();
        }

        /// <summary>Read a json element of number type.</summary>
        /// <param name="text">The string to be read.</param>
        /// <param name="index">The starting position for reading.</param>
        /// <returns>Return a successfully read number type json element.</returns>
        private static JsonNumber ReadJsonNumber(string text, ref int index)
        {
            var i = index;
            while (i < text.Length && char.IsNumber(text[i]) || text[i] == '.') i++;
            if (double.TryParse(text.Substring(index - 1, i - index + 1), out var value))
            {
                index = i;
                return new JsonNumber(value);
            }
            throw new JsonParseException($"Unrecognizable number type, Index position {index}.");
        }

    }

    internal class JsonString : JsonElement
    {
        public JsonString(string value) { Value = value; JEleType = JsonElementType.String; }
        public override string ToString() => $"\"{Value}\"";
    }

    internal class JsonNumber : JsonElement, IEquatable<JsonNumber>
    {
        public JsonNumber(double value) { Value = value; JEleType = JsonElementType.Number; }

        public bool Equals(JsonNumber other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }
        public override bool Equals(object obj) => (obj.GetType() != this.GetType()) ? false : Equals((JsonNumber)obj);
    }

    internal class JsonBoolean : JsonElement, IEquatable<JsonBoolean>
    {
        public JsonBoolean(bool value) { Value = value; JEleType = JsonElementType.Boolean; }
        public JsonBoolean(string value)
        {
            string v = value.ToLower();
            if (v != "true" && v != "false")
                throw new JsonParseException($"{value} cannot be converted to a valid Boolean type.");
            Value = bool.Parse(value);
            JEleType = JsonElementType.Boolean;
        }

        public bool Equals(JsonBoolean other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }
        public override bool Equals(object obj) => (obj.GetType() != this.GetType()) ? false : Equals((JsonBoolean)obj);

    }

    internal class JsonNull : JsonElement, IEquatable<JsonNull>
    {
        public override string ToString() { return "null"; }

        public bool Equals(JsonNull obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj != null;
        }
        public override bool Equals(object obj) => (obj.GetType() != this.GetType()) ? false : Equals((JsonNull)obj);

        public override int GetHashCode() => "null".GetHashCode();
    }

    internal class JsonObject : JsonElement, IDictionary<string, JsonElement>, IEquatable<JsonObject>
    {
        private IDictionary<string, JsonElement> _propertyMap;
        public JsonObject() { Value = _propertyMap = new Dictionary<string, JsonElement>(); JEleType = JsonElementType.Object; }

        public bool ContainsKey(string key) => _propertyMap.ContainsKey(key);

        public void Add(string key, JsonElement value) => _propertyMap.Add(key, value);

        public bool Remove(string key) => _propertyMap.Remove(key);

        public bool TryGetValue(string key, out JsonElement value) => _propertyMap.TryGetValue(key, out value);

        public JsonElement this[string name] { get => _propertyMap[name]; set => _propertyMap[name] = value; }

        public ICollection<string> Keys => _propertyMap.Keys;
        public ICollection<JsonElement> Values => _propertyMap.Values;

        public IEnumerator<KeyValuePair<string, JsonElement>> GetEnumerator() => _propertyMap.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValuePair<string, JsonElement> item) => _propertyMap.Add(item.Key, item.Value);

        public void Clear() => _propertyMap.Clear();

        public bool Contains(KeyValuePair<string, JsonElement> item) => _propertyMap.Contains(item);

        public void CopyTo(KeyValuePair<string, JsonElement>[] array, int arrayIndex) => _propertyMap.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<string, JsonElement> item) => _propertyMap.Remove(item.Key);

        public int Count => _propertyMap.Count;
        public bool IsReadOnly => false;

        public bool Equals(JsonObject other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.Count != this.Count) return false;
            foreach (var property in this._propertyMap)
            {
                if (!other.TryGetValue(property.Key, out var value) || !value.Equals(property.Value))
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj) => (obj.GetType() != this.GetType()) ? false : Equals((JsonObject)obj);

        public override int GetHashCode()
        {
            return _propertyMap.Aggregate(0, (current, jsonElement) => current ^ jsonElement.Key.GetHashCode() ^ jsonElement.Value.GetHashCode());
        }

        public override string ToString() => $"{{{string.Join(",", _propertyMap.Select(x => $"\"{x.Key}\":{x.Value}"))}}}";
    }

    internal class JsonArray : JsonElement, IList<JsonElement>, IEquatable<JsonArray>
    {
        private List<JsonElement> _Elements;
        public JsonArray() { Value = _Elements = new List<JsonElement>(); JEleType = JsonElementType.Array; }

        public IEnumerator<JsonElement> GetEnumerator() => _Elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(JsonElement item) => _Elements.Add(item);

        public void Clear() => _Elements.Clear();

        public bool Contains(JsonElement item) => _Elements.Contains(item);

        public void CopyTo(JsonElement[] array, int arrayIndex) => _Elements.CopyTo(array, arrayIndex);

        public bool Remove(JsonElement item) => _Elements.Remove(item);

        public int Count => _Elements.Count;
        public bool IsReadOnly => false;
        public int IndexOf(JsonElement item) => _Elements.IndexOf(item);

        public void Insert(int index, JsonElement item) => _Elements.Insert(index, item);

        public void RemoveAt(int index) => _Elements.RemoveAt(index);

        public JsonElement this[int index] { get => _Elements[index]; set => _Elements[index] = value; }

        public override string ToString() => $"[{string.Join(",", this)}]";

        public bool Equals(JsonArray other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return !other._Elements.Where((t, i) => !t.Equals(this._Elements[i])).Any();
        }

        public override bool Equals(object obj) => (obj.GetType() != this.GetType()) ? false : Equals((JsonArray)obj);

        public override int GetHashCode() => _Elements.Aggregate(0, (current, jsonElement) => current ^ jsonElement.GetHashCode());
    }

    internal class JsonParseException : Exception
    {
        public JsonParseException() { }
        public JsonParseException(string message) : base(message) { }
        public JsonParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
