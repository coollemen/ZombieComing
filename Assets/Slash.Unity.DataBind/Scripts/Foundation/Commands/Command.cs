// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Command.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;

    using UnityEngine;

    /// <summary>
    ///   Base class for a command which invokes a method in a data context.
    /// </summary>
    public class Command : MonoBehaviour, IContextOperator
    {
        #region Fields

        /// <summary>
        ///   Additional arguments to pass when command is invoked.
        /// </summary>
        public DataProvider[] AdditionalArguments;

        /// <summary>
        ///   Path of method to call in data context.
        /// </summary>
        [ContextPath(Filter = ContextMemberFilter.Methods | ContextMemberFilter.Recursive)]
        public string Path;

        private Delegate command;

        private ContextNode node;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Invokes the bound method.
        /// </summary>
        public void InvokeCommand()
        {
            this.InvokeCommand(new object[]{});
        }

        /// <summary>
        ///   Invokes the bound method with the specified arguments.
        /// </summary>
        /// <param name="args">Arguments to invoke the bound method with.</param>
        public void InvokeCommand(params object[] args)
        {
            if (this.command == null)
            {
                return;
            }

            // Add additional arguments if there are any.
            var commandArgs = args;
            var additionalArgCount = this.AdditionalArguments.Length;
            if (additionalArgCount > 0)
            {
                var argList = new List<object>();
                argList.AddRange(args);
                argList.AddRange(
                    this.AdditionalArguments.Select(
                        additionArgument => additionArgument != null ? additionArgument.Value : null));
                commandArgs = argList.ToArray();
            }

            // Use default parameters if more are required than provided.
            var parameterInfos = this.command.Method.GetParameters();
            if (parameterInfos.Length > commandArgs.Length)
            {
                var argList = new List<object>();
                argList.AddRange(commandArgs);
                for (var index = commandArgs.Length; index < parameterInfos.Length; index++)
                {
                    var parameterInfo = parameterInfos[index];
                    var defaultValue = parameterInfo.ParameterType.IsValueType
                        ? Activator.CreateInstance(parameterInfo.ParameterType)
                        : null;
                    argList.Add(defaultValue);
                }
                commandArgs = argList.ToArray();
            }
            // Skip base arguments if less are required.
            else if (parameterInfos.Length < commandArgs.Length)
            {
                var argList = new List<object>();

                var baseArgCount = parameterInfos.Length - additionalArgCount;
                for (var index = 0; index < baseArgCount; index++)
                {
                    argList.Add(args[index]);
                }

                // Add additional arguments.
                argList.AddRange(
                    this.AdditionalArguments.Select(
                        additionArgument => additionArgument != null ? additionArgument.Value : null));

                commandArgs = argList.ToArray();
            }

            try
            {
                // Invoke delegate.
                this.command.DynamicInvoke(commandArgs);
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is TargetParameterCountException)
                {
                    Debug.LogError(
                        string.Format(
                            "Couldn't invoke command '{0}' with arguments: [{1}]. (Exception: {2})",
                            this.Path,
                            commandArgs.Aggregate(
                                string.Empty,
                                (text, arg) =>
                                    (string.IsNullOrEmpty(text) ? string.Empty : (text + ", "))
                                    + (arg != null ? arg.ToString() : "null")),
                            e),
                        this);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///   Has to be called when an anchestor context changed as the data value may change.
        /// </summary>
        public void OnContextChanged()
        {
            if (this.node != null)
            {
                this.node.OnHierarchyChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMemberHiearchy.Global")]
        protected virtual void Awake()
        {
            this.node = new ContextNode(this.gameObject, this.Path);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.node.SetValueListener(null);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
        protected virtual void Start()
        {
            // Monitor command.
            this.command = this.node.SetValueListener(this.OnCommandChanged) as Delegate;
        }

        private void OnCommandChanged(object obj)
        {
            this.command = obj as Delegate;
        }

        #endregion
    }
}