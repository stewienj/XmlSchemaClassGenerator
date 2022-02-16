﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSchemaClassGenerator
{
    public abstract class RestrictionModel
    {
        public GeneratorConfiguration Configuration { get; private set; }

        protected RestrictionModel(GeneratorConfiguration configuration)
        {
            Configuration = configuration;
        }

        public bool IsSupported
        {
            get
            {
                return MinimumDataAnnotationMode >= Configuration.DataAnnotationMode;
            }
        }

        /// <summary>
        /// The DataAnnotationMode required to be able to emit this restriction
        /// </summary>
        public abstract DataAnnotationMode MinimumDataAnnotationMode { get; }
        public abstract string Description { get; }
        public abstract CodeAttributeDeclaration GetAttribute();
    }

    public abstract class ValueRestrictionModel<T> : RestrictionModel
    {
        protected ValueRestrictionModel(GeneratorConfiguration configuration)
            : base(configuration)
        {

        }

        public T Value { get; set; }

    }

    public abstract class ValueTypeRestrictionModel: ValueRestrictionModel<string>
    {
        protected ValueTypeRestrictionModel(GeneratorConfiguration configuration)
            : base(configuration)
        {

        }

        public Type Type { get; set; }
    }

    public class MinMaxLengthRestrictionModel : RestrictionModel
    {
        public MinMaxLengthRestrictionModel(GeneratorConfiguration configuration)
            : base(configuration)
        {

        }

        public int Min { get; set; }
        public int Max { get; set; }

        public override string Description
        {
            get
            {
                var s = "";
                if (Min > 0) { s += string.Format("Minimum length: {0}. ", Min); }
                if (Max > 0) { s += string.Format("Maximum length: {0}.", Max); }
                return s.Trim();
            }
        }

        public override DataAnnotationMode MinimumDataAnnotationMode
        {
            get { return DataAnnotationMode.Partial; }
        }

        public override CodeAttributeDeclaration GetAttribute()
        {
            var a = new CodeAttributeDeclaration(CodeUtilities.CreateTypeReference("System.ComponentModel.DataAnnotations", "StringLengthAttribute", Configuration),
                new CodeAttributeArgument(Max > 0 ? (CodeExpression)new CodePrimitiveExpression(Max) : new CodeSnippetExpression("int.MaxValue")));
            if (Min > 0) { a.Arguments.Add(new CodeAttributeArgument("MinimumLength", new CodePrimitiveExpression(Min))); }

            return a;
        }
    }

    public class MaxLengthRestrictionModel : ValueRestrictionModel<int>
    {
        public MaxLengthRestrictionModel(GeneratorConfiguration configuration)
            : base(configuration)
        {

        }

        public override string Description
        {
            get
            {
                return string.Format("Maximum length: {0}.", Value);
            }
        }

        public override DataAnnotationMode MinimumDataAnnotationMode
        {
            get { return DataAnnotationMode.All; }
        }

        public override CodeAttributeDeclaration GetAttribute()
        {
            return new CodeAttributeDeclaration(CodeUtilities.CreateTypeReference("System.ComponentModel.DataAnnotations", "MaxLengthAttribute", Configuration),
                new CodeAttributeArgument(new CodePrimitiveExpression(Value)));
        }
    }

    public class MinLengthRestrictionModel : ValueRestrictionModel<int>
    {
        public MinLengthRestrictionModel(GeneratorConfiguration configuration)
            : base(configuration)
        {

        }

        public override string Description
        {
            get
            {
                return string.Format("Minimum length: {0}.", Value);
            }
        }

        public override DataAnnotationMode MinimumDataAnnotationMode
        {
            get { return DataAnnotationMode.All; }
        }

        public override CodeAttributeDeclaration GetAttribute()
        {
            return new CodeAttributeDeclaration(CodeUtilities.CreateTypeReference("System.ComponentModel.DataAnnotations", "MinLengthAttribute", Configuration),
                new CodeAttributeArgument(new CodePrimitiveExpression(Value)));
        }
    }

    public class TotalDigitsRestrictionModel : ValueRestrictionModel<int>
    {
        public TotalDigitsRestrictionModel(GeneratorConfiguration configuration)
            : base(configuration)
        {

        }

        public override DataAnnotationMode MinimumDataAnnotationMode
        {
            get { return DataAnnotationMode.All; }
        }

        public override string Description
        {
            get
            {
                return string.Format("Total number of digits: {0}.", Value);
            }
        }

        public override CodeAttributeDeclaration GetAttribute()
        {
            return new CodeAttributeDeclaration(CodeUtilities.CreateTypeReference("CustomDataAnnotations", "TotalDigitsAttribute", Configuration),
                new CodeAttributeArgument(new CodePrimitiveExpression(Value)));
        }
    }

    public class FractionDigitsRestrictionModel : ValueRestrictionModel<int>
    {
        public FractionDigitsRestrictionModel(GeneratorConfiguration configuration)
            : base(configuration)
        {

        }

        public override DataAnnotationMode MinimumDataAnnotationMode
        {
            get { return DataAnnotationMode.All; }
        }

        public override string Description
        {
            get
            {
                return string.Format("Total number of digits in fraction: {0}.", Value);
            }
        }

        public override CodeAttributeDeclaration GetAttribute()
        {
            return new CodeAttributeDeclaration(CodeUtilities.CreateTypeReference("CustomDataAnnotations", "FractionDigitsAttribute", Configuration),
                new CodeAttributeArgument(new CodePrimitiveExpression(Value)));
        }

    }

    public class PatternRestrictionModel : ValueRestrictionModel<string>
    {
        public PatternRestrictionModel(GeneratorConfiguration configuration)
            : base(configuration)
        {

        }

        public override string Description
        {
            get
            {
                return string.Format("Pattern: {0}.", Value);
            }
        }

        public override DataAnnotationMode MinimumDataAnnotationMode
        {
            get { return DataAnnotationMode.Partial; }
        }

        public override CodeAttributeDeclaration GetAttribute()
        {
            return new CodeAttributeDeclaration(CodeUtilities.CreateTypeReference("System.ComponentModel.DataAnnotations", "RegularExpressionAttribute", Configuration),
                new CodeAttributeArgument(new CodePrimitiveExpression(Value)));
        }
    }

    public class MinInclusiveRestrictionModel: ValueTypeRestrictionModel
    {
        public MinInclusiveRestrictionModel(GeneratorConfiguration configuration)
            : base(configuration)
        {

        }

        public override DataAnnotationMode MinimumDataAnnotationMode
        {
            get { return DataAnnotationMode.All; }
        }

        public override string Description
        {
            get
            {
                return string.Format("Minimum inclusive value: {0}.", Value);
            }
        }

        public override CodeAttributeDeclaration GetAttribute()
        {
            return new CodeAttributeDeclaration(CodeUtilities.CreateTypeReference("CustomDataAnnotations", "MinInclusiveAttribute", Configuration),
                new CodeAttributeArgument(new CodePrimitiveExpression(Value)));
        }

    }

    public class MinExclusiveRestrictionModel: ValueTypeRestrictionModel
    {
        public MinExclusiveRestrictionModel(GeneratorConfiguration configuration)
            : base(configuration)
        {

        }

        public override DataAnnotationMode MinimumDataAnnotationMode
        {
            get { return DataAnnotationMode.All; }
        }

        public override string Description
        {
            get
            {
                return string.Format("Minimum exclusive value: {0}.", Value);
            }
        }

        public override CodeAttributeDeclaration GetAttribute()
        {
            return new CodeAttributeDeclaration(CodeUtilities.CreateTypeReference("CustomDataAnnotations", "MinExclusiveAttribute", Configuration),
                new CodeAttributeArgument(new CodePrimitiveExpression(Value)));
        }

    }

    public class MaxInclusiveRestrictionModel: ValueTypeRestrictionModel
    {
        public MaxInclusiveRestrictionModel(GeneratorConfiguration configuration)
            : base(configuration)
        {

        }

        public override DataAnnotationMode MinimumDataAnnotationMode
        {
            get { return DataAnnotationMode.All; }
        }

        public override string Description
        {
            get
            {
                return string.Format("Maximum inclusive value: {0}.", Value);
            }
        }

        public override CodeAttributeDeclaration GetAttribute()
        {
            return new CodeAttributeDeclaration(CodeUtilities.CreateTypeReference("CustomDataAnnotations", "MaxInclusiveAttribute", Configuration),
                new CodeAttributeArgument(new CodePrimitiveExpression(Value)));
        }

    }

    public class MaxExclusiveRestrictionModel: ValueTypeRestrictionModel
    {
        public MaxExclusiveRestrictionModel(GeneratorConfiguration configuration)
            : base(configuration)
        {

        }

        public override DataAnnotationMode MinimumDataAnnotationMode
        {
            get { return DataAnnotationMode.All; }
        }

        public override string Description
        {
            get
            {
                return string.Format("Maximum exclusive value: {0}.", Value);
            }
        }

        public override CodeAttributeDeclaration GetAttribute()
        {
            return new CodeAttributeDeclaration(CodeUtilities.CreateTypeReference("CustomDataAnnotations", "MaxExclusiveAttribute", Configuration),
                new CodeAttributeArgument(new CodePrimitiveExpression(Value)));
        }

    }
}
