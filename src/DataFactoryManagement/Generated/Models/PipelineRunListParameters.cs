// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using System;
using System.Linq;

namespace Microsoft.Azure.Management.DataFactories.Models
{
    /// <summary>
    /// Parameters specifying the activity, time range, and other filters for
    /// the get runs operation.
    /// </summary>
    public partial class PipelineRunListParameters
    {
        private string _activityName;
        
        /// <summary>
        /// Required. A unique activity name.
        /// </summary>
        public string ActivityName
        {
            get { return this._activityName; }
            set { this._activityName = value; }
        }
        
        private string _runRangeEndTime;
        
        /// <summary>
        /// Required. The run range end time in round-trip ISO 8601 format.
        /// </summary>
        public string RunRangeEndTime
        {
            get { return this._runRangeEndTime; }
            set { this._runRangeEndTime = value; }
        }
        
        private string _runRangeStartTime;
        
        /// <summary>
        /// Required. The run range start time in round-trip ISO 8601 format.
        /// </summary>
        public string RunRangeStartTime
        {
            get { return this._runRangeStartTime; }
            set { this._runRangeStartTime = value; }
        }
        
        private string _runRecordStatus;
        
        /// <summary>
        /// Optional. A run status to filter by.
        /// </summary>
        public string RunRecordStatus
        {
            get { return this._runRecordStatus; }
            set { this._runRecordStatus = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the PipelineRunListParameters class.
        /// </summary>
        public PipelineRunListParameters()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the PipelineRunListParameters class
        /// with required arguments.
        /// </summary>
        public PipelineRunListParameters(string activityName, string runRangeStartTime, string runRangeEndTime)
            : this()
        {
            if (activityName == null)
            {
                throw new ArgumentNullException("activityName");
            }
            if (runRangeStartTime == null)
            {
                throw new ArgumentNullException("runRangeStartTime");
            }
            if (runRangeEndTime == null)
            {
                throw new ArgumentNullException("runRangeEndTime");
            }
            this.ActivityName = activityName;
            this.RunRangeStartTime = runRangeStartTime;
            this.RunRangeEndTime = runRangeEndTime;
        }
    }
}