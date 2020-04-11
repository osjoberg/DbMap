using System;
using System.Data.Common;
using System.Reflection;

namespace DbMap.Deserialization
{
    internal class AdoProviderMetadata
    {
        private const BindingFlags PublicInstanceDeclaredOnly = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;

        private static readonly Type[] ParameterConstructorParameters = { typeof(string), typeof(object) };

        private static AdoProviderMetadata[] adoProviderMetadataItems = { };

        private readonly Type connectionType;

        private AdoProviderMetadata(Type connectionType)
        {
            this.connectionType = connectionType;

            var createCommandMethod = GetPublicInstanceDeclaredOnlyMethod(connectionType, nameof(DbConnection.CreateCommand));
            if (createCommandMethod == null)
            {
                throw new NotSupportedException();
            }

            var commandType = createCommandMethod.ReturnType;

            var parametersProperty = commandType.GetProperty(nameof(DbCommand.Parameters), PublicInstanceDeclaredOnly);
            if (parametersProperty == null)
            {
                throw new NotSupportedException();
            }

            var parameterCollectionType = parametersProperty.PropertyType;

            var executeReaderMethod = GetPublicInstanceDeclaredOnlyMethod(commandType, nameof(DbCommand.ExecuteReader));
            if (executeReaderMethod == null)
            {
                throw new NotSupportedException();
            }

            var dataReaderType = executeReaderMethod.ReturnType;

            var createParameterMethod = GetPublicInstanceDeclaredOnlyMethod(commandType, nameof(DbCommand.CreateParameter));
            if (createParameterMethod == null)
            {
                throw new NotSupportedException();
            }

            var parameterType = createParameterMethod.ReturnType;

            ParameterConstructor = parameterType.GetConstructor(ParameterConstructorParameters);
            if (ParameterConstructor == null)
            {
                throw new NotSupportedException();
            }

            ParameterCollectionAddMethod = parameterCollectionType.GetMethod(nameof(DbParameterCollection.Add), new[] { parameterType });
            if (ParameterCollectionAddMethod == null)
            {
                throw new NotSupportedException();
            }

            DataReaderMetadata = new DataReaderMetadata(dataReaderType);
            if (DataReaderMetadata == null)
            {
                throw new NotSupportedException();
            }
        }

        public ConstructorInfo ParameterConstructor { get; }

        public MethodInfo ParameterCollectionAddMethod { get; }

        public DataReaderMetadata DataReaderMetadata { get; }

        public static AdoProviderMetadata GetMetadata(Type connectionType)
        {
            var adoProviderMetadataItemsCopy = adoProviderMetadataItems;

            for (var index = 0; index < adoProviderMetadataItemsCopy.Length; index++)
            {
                var adoProvideMetadata = adoProviderMetadataItemsCopy[index];

                if (ReferenceEquals(adoProvideMetadata.connectionType, connectionType))
                {
                    return adoProvideMetadata;
                }
            }

            var newAdoProviderMetadataItems = new AdoProviderMetadata[adoProviderMetadataItemsCopy.Length + 1];
            Array.Copy(adoProviderMetadataItemsCopy, 0, newAdoProviderMetadataItems, 1, adoProviderMetadataItemsCopy.Length);
            newAdoProviderMetadataItems[0] = new AdoProviderMetadata(connectionType);

            adoProviderMetadataItems = newAdoProviderMetadataItems;

            return newAdoProviderMetadataItems[0];
        }

        private static MethodInfo GetPublicInstanceDeclaredOnlyMethod(Type type, string name)
        {
            return type.GetMethod(name, PublicInstanceDeclaredOnly, null, CallingConventions.Any, Type.EmptyTypes, null);
        }
    }
}
