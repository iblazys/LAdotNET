// This file is part of LAdotNET.
//
// LAdotNET is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// LAdotNET is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with LAdotNET.  If not, see <https://www.gnu.org/licenses/>.

namespace LAdotNET.Network.Encryption
{
    public class Counter
    {
        private readonly byte maxCount;
        private readonly byte minCount;
        private readonly object lockObject = new object();
        private int counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Counter"/> class.
        /// </summary>
        public Counter()
        {
            this.maxCount = 255;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Counter"/> class.
        /// </summary>
        /// <param name="min">The minimum counter value.</param>
        /// <param name="max">The maximum counter value.</param>
        public Counter(byte min, byte max)
        {
            this.minCount = min;
            this.maxCount = max;
        }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                lock (this.lockObject)
                {
                    return this.counter;
                }
            }

            set
            {
                lock (this.lockObject)
                {
                    this.counter = value;
                }
            }
        }

        /// <summary>
        /// Increases the counter value.
        /// When the maximum count is getting exceeded,
        /// the counter will jump back to the minimum counter value.
        /// </summary>
        public void Increase()
        {
            lock (this.lockObject)
            {
                if (this.counter == this.maxCount)
                {
                    this.counter = this.minCount;
                }
                else
                {
                    this.counter++;
                }
            }
        }

        /// <summary>
        /// Resets the count to the minimum value.
        /// </summary>
        public void Reset()
        {
            this.Count = this.minCount;
        }
    }
}
