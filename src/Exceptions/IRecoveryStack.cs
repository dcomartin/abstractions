﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.


namespace Unity.Exceptions
{
    /// <summary>
    /// Data structure that stores the set of <see cref="IRequiresRecovery"/>
    /// objects and executes them when requested.
    /// </summary>
    public interface IRecoveryStack
    {
        /// <summary>
        /// Add a new <see cref="IRequiresRecovery"/> object to this
        /// list.
        /// </summary>
        /// <param name="recovery">Object to add.</param>
        void Add(IRequiresRecovery recovery);

        /// <summary>
        /// Return the number of recovery objects currently in the stack.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Execute the <see cref="IRequiresRecovery.Recover"/> pipeline
        /// of everything in the recovery list. Recoveries will execute
        /// in the opposite order of add - it's a stack.
        /// </summary>
        void ExecuteRecovery();
    }
}
