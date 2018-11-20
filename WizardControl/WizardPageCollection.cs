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
                get => (WizardPage)controls[index + owner.FirstPageIndex];
                set
                {
                    Insert(index + owner.FirstPageIndex, value);
                    RemoveAt(index + owner.FirstPageIndex + 1);
                }
            }
            public int Count => owner.PageCount;
            public bool IsReadOnly => false;
            #endregion

            #region Constructor
            public WizardPageCollection(WizardControl control)
            {
                owner = control;
                controls = control.Controls;
            }
            #endregion

            #region Public Methods
            public void Add(WizardPage item)
            {
                controls.Add(item);
                if (Count == 1) owner.SelectedIndex = 0;

                owner.UpdateNavigationControls();
                owner.UpdatePages();
            }

            public void Clear()
            {
                List<Control> toRemove = new List<Control>();
                for (int i = owner.FirstPageIndex; i < owner.FirstPageIndex + owner.PageCount; i++)
                    toRemove.Add(controls[i]);
                foreach (Control control in toRemove)
                    controls.Remove(control);
                owner.SelectedIndex = -1;
                owner.UpdateNavigationControls();
            }

            public bool Contains(WizardPage item)
            {
                return controls.Contains(item);
            }

            public void CopyTo(WizardPage[] array, int arrayIndex)
            {
                for (int i = arrayIndex; i < array.Length; i++)
                    array[i] = (WizardPage)controls[i - arrayIndex + owner.FirstPageIndex];
            }

            public IEnumerator<WizardPage> GetEnumerator()
            {
                for (int i = owner.FirstPageIndex; i < owner.FirstPageIndex + owner.PageCount; i++)
                    yield return (WizardPage)controls[i];
            }

            public int IndexOf(WizardPage item)
            {
                return controls.IndexOf(item) - owner.FirstPageIndex;
            }

            public void Insert(int index, WizardPage item)
            {
                index += owner.FirstPageIndex;
                List<Control> removed = new List<Control>();
                for (int i = controls.Count - 1; i >= index; i--)
                {
                    removed.Add(controls[i]);
                    controls.RemoveAt(i);
                }
                controls.Add(item);
                for (int i = removed.Count - 1; i >= 0; i--)
                {
                    controls.Add(removed[i]);
                }
                if (Count == 1) owner.SelectedIndex = 0;

                owner.UpdateNavigationControls();
                owner.UpdatePages();
            }

            public bool Remove(WizardPage item)
            {
                bool exists = controls.Contains(item);

                controls.Remove(item);

                if (Count == 0)
                    owner.SelectedIndex = -1;
                else if (owner.SelectedIndex > Count - 1)
                    owner.SelectedIndex = 0;

                owner.UpdateNavigationControls();
                owner.UpdatePages();

                return exists;
            }

            public void RemoveAt(int index)
            {
                controls.RemoveAt(index + owner.FirstPageIndex);

                if (Count == 0)
                    owner.SelectedIndex = -1;
                else if (owner.SelectedIndex > Count - 1)
                    owner.SelectedIndex = 0;

                owner.UpdateNavigationControls();
                owner.UpdatePages();
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
