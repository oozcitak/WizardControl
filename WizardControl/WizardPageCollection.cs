using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class WizardControl
    {
        public class WizardPageCollection : IList<WizardPage>, ICollection, IList, IEnumerable
        {
            #region Member Variables
            private WizardControl owner;
            private ControlCollection controls;
            #endregion

            #region Properties
            public WizardPage this[int index]
            {
                get => (WizardPage)controls[index];
                set
                {
                    Insert(index, value);
                    RemoveAt(index + 1);
                }
            }
            public int Count => controls.Count;
            public bool IsReadOnly => controls.IsReadOnly;
            #endregion

            #region Constructor
            public WizardPageCollection(WizardControl control)
            {
                owner = control;
                controls = control.pageContainer.Controls;
            }
            #endregion

            #region Public Methods
            public void Add(WizardPage item)
            {
                controls.Add(item);
                owner.UpdateNavigationControls();
            }

            public void Clear()
            {
                controls.Clear();
                owner.UpdateNavigationControls();
            }

            public bool Contains(WizardPage item)
            {
                return controls.Contains(item);
            }

            public void CopyTo(WizardPage[] array, int arrayIndex)
            {
                controls.CopyTo(array, arrayIndex);
            }

            public IEnumerator<WizardPage> GetEnumerator()
            {
                var iterator = controls.GetEnumerator();
                while (iterator.MoveNext())
                {
                    yield return (WizardPage)iterator.Current;
                }
            }

            public int IndexOf(WizardPage item)
            {
                return controls.IndexOf(item);
            }

            public void Insert(int index, WizardPage item)
            {
                controls.Add(item);

                List<Control> removed = new List<Control>();
                for (int i = controls.Count - 2; i >= index; i--)
                {
                    removed.Add(controls[i]);
                    controls.RemoveAt(i);
                }
                for (int i = removed.Count - 1; i >= 0; i--)
                {
                    controls.Add(removed[i]);
                }

                owner.UpdateNavigationControls();
            }

            public bool Remove(WizardPage item)
            {
                bool exists = controls.Contains(item);

                controls.Remove(item);

                owner.UpdateNavigationControls();

                return exists;
            }

            public void RemoveAt(int index)
            {
                controls.RemoveAt(index);
                owner.UpdateNavigationControls();
            }
            #endregion

            #region Explicit Interface
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            object ICollection.SyncRoot => throw new NotImplementedException();

            bool ICollection.IsSynchronized => false;

            void ICollection.CopyTo(Array array, int index) => throw new NotImplementedException();

            bool IList.IsFixedSize => false;

            object IList.this[int index] { get => this[index]; set => this[index] = (WizardPage)value; }

            int IList.Add(object value)
            {
                Add((WizardPage)value);
                return Count - 1;
            }

            bool IList.Contains(object value)
            {
                return Contains((WizardPage)value);
            }

            int IList.IndexOf(object value)
            {
                return IndexOf((WizardPage)value);
            }

            void IList.Insert(int index, object value)
            {
                Insert(index, (WizardPage)value);
            }

            void IList.Remove(object value)
            {
                Remove((WizardPage)value);
            }
            #endregion
        }
    }
}
