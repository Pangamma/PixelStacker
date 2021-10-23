using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.UI.Forms
{



    /// <summary>
    /// CustomClass (Which is binding to property grid)
    /// </summary>
    public class CustomGridView : CollectionBase, ICustomTypeDescriptor
    {
        //private static bool TryGetHtmlPropertiesFromModel<TModel, TProperty>(Expression<Func<TModel, TProperty>> exprForName, out string htmlFieldName, out string htmlFieldValue, out string resolvedLabelText, out ModelMetadata metadata)
        //{
        //    htmlFieldName = null;
        //    htmlFieldValue = null;
        //    resolvedLabelText = null;
        //    metadata = null;

        //    try
        //    {
        //        exprForName.Compile();
        //        metadata = ModelMetadata.FromLambdaExpression(exprForName, html.ViewData);
        //        htmlFieldName = ExpressionHelper.GetExpressionText(exprForName);
        //        resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
        //        htmlFieldValue = exprForName.Compile()(html.ViewData.Model)?.ToString();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Telemetry.Instance.WriteException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //    }
        //    return false;
        //}

        public void Add<TModel, TProp>(TModel model, Expression<Func<TModel, TProp>> expr, bool isReadonly = false, bool isVisible = true)
        {
            if (expr.Body is MemberExpression expression)
            {
                var fieldName = expression.Member.Name;
                var propertyInfo = (PropertyInfo)expression.Member;
                var propertyType = propertyInfo.PropertyType;
                var propertyName = propertyInfo.Name;
                var attributes = expression.Member.GetCustomAttributes(true);
                string category = attributes.OfType<CategoryAttribute>().Select(x => x.Category).FirstOrDefault() ?? "";
                string dispName = attributes.OfType<DisplayNameAttribute>().Select(x => x.DisplayName).FirstOrDefault() ?? fieldName;
                string description = attributes.OfType<DescriptionAttribute>().Select(x => x.Description).FirstOrDefault() ?? fieldName;
                isReadonly |= attributes.OfType<ReadOnlyAttribute>().Any();
                string propTab = attributes.OfType<PropertyTabAttribute>().Select(x => "Foobar").FirstOrDefault();

                var val = expr.Compile().Invoke(model);

                base.List.Add(new CustomProperty()
                {
                    Category = category,
                    PropertyTab = propTab,
                    DisplayName = dispName,
                    Description = description,
                    Name = fieldName,
                    ReadOnly = isReadonly,
                    Value = val,
                    Visible = isVisible,
                    Type = propertyType
                });
            }
        }

        /// <summary>
        /// Add CustomProperty to Collectionbase List
        /// </summary>
        /// <param name="Value"></param>
        public void Add(CustomProperty Value)
        {
            base.List.Add(Value);
        }

        /// <summary>
        /// Remove item from List
        /// </summary>
        /// <param name="Name"></param>
        public void Remove(string Name)
        {
            foreach (CustomProperty prop in base.List)
            {
                if (prop.Name == Name)
                {
                    base.List.Remove(prop);
                    return;
                }
            }
        }

        /// <summary>
        /// Indexer
        /// </summary>
        public CustomProperty this[int index]
        {
            get
            {
                return (CustomProperty)base.List[index];
            }
            set
            {
                base.List[index] = (CustomProperty)value;
            }
        }


        #region "TypeDescriptor Implementation"
        /// <summary>
        /// Get Class Name
        /// </summary>
        /// <returns>String</returns>
        public String GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        /// <summary>
        /// GetAttributes
        /// </summary>
        /// <returns>AttributeCollection</returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        /// <summary>
        /// GetComponentName
        /// </summary>
        /// <returns>String</returns>
        public String GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        /// <summary>
        /// GetConverter
        /// </summary>
        /// <returns>TypeConverter</returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        /// <summary>
        /// GetDefaultEvent
        /// </summary>
        /// <returns>EventDescriptor</returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        /// <summary>
        /// GetDefaultProperty
        /// </summary>
        /// <returns>PropertyDescriptor</returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        /// <summary>
        /// GetEditor
        /// </summary>
        /// <param name="editorBaseType">editorBaseType</param>
        /// <returns>object</returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptor[] newProps = new PropertyDescriptor[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                CustomProperty prop = (CustomProperty)this[i];
                newProps[i] = new CustomPropertyDescriptor(ref prop, attributes);
            }

            return new PropertyDescriptorCollection(newProps);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
        #endregion

    }

    /// <summary>
    /// Custom property class 
    /// </summary>
    public class CustomProperty
    {
        public CustomProperty() { }

        public Type Type { get; set; }

        public string Category { get; set; } = String.Empty;
        public bool ReadOnly { get; set; } = false;

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public bool Visible { get; set; }

        public object Value { get; set; }
        public string PropertyTab { get; set; }
    }


    /// <summary>
    /// Custom PropertyDescriptor
    /// </summary>
    public class CustomPropertyDescriptor : PropertyDescriptor
    {
        CustomProperty m_Property;
        public CustomPropertyDescriptor(ref CustomProperty myProperty, Attribute[] attrs) : base(myProperty.Name, attrs)
        {
            m_Property = myProperty;
        }

        #region PropertyDescriptor specific

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override object GetValue(object component)
        {
            return m_Property.Value;
        }

        public override string Description
        {
            get { return m_Property.Description; }
        }

        public override string Category
        {
            get { return m_Property.Category ?? string.Empty; }
        }

        public override string DisplayName
        {
            get { return m_Property.DisplayName; }
        }

        public override bool IsReadOnly
        {
            get { return m_Property.ReadOnly; }
        }

        public override void ResetValue(object component)
        {
            //Have to implement
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override void SetValue(object component, object value)
        {
            m_Property.Value = value;
        }

        public override Type PropertyType
        {
            get { return m_Property.Type; }
        }

        #endregion

    }
}
