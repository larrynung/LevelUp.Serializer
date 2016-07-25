namespace LevelUp.Serializer
{
    /// <summary>
    ///
    /// </summary>
    public enum SerializerType
    {
        /// <summary>
        ///
        /// </summary>
        None,

        /// <summary>
        ///
        /// </summary>
        [RelativedType(typeof(BinarySerializer))]
        Binary,

        /// <summary>
        ///
        /// </summary>
        [RelativedType(typeof(XmlSerializer))]
        Xml,

        /// <summary>
        ///
        /// </summary>
        [RelativedType(typeof(SoapSerializer))]
        Soap,

        /// <summary>
        ///
        /// </summary>
        [RelativedType(typeof(JsonSerializer))]
        Json
    }
}