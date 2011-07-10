using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBeePlugin
{
    partial class Plugin
    {
        partial class MusicBeeAPI
        {
            partial class FileInfo
            {
                private class MultiTagList : IList<string>
                {
                    private FileInfo file;
                    private MetaDataType tagType;
                    private List<string> list;
                    private bool isDirty;

                    public MultiTagList(FileInfo file, MetaDataType tagType) {
                        this.file = file;
                        this.tagType = tagType;
                        string tag = file.mb.Library_GetFileTag(file.ToString(), tagType);
                        list = new List<string>(tag.Split(new char[] { '\0' }));
                        isDirty = false;
                    }

                    public void Commit() {
                        if (isDirty) {
                            string tag;
                            if (list.Count > 0) {
                                tag = String.Join("\0", list.ToArray());
                            } else {
                                tag = "";
                            }
                            file.mb.Library_SetFileTag(file.ToString(), tagType, tag);
                            isDirty = false;
                        }
                    }

                    int IList<string>.IndexOf(string item) {
                        return list.IndexOf(item);
                    }

                    void IList<string>.Insert(int index, string item) {
                        list.Insert(index, item);
                        isDirty = true;
                    }

                    void IList<string>.RemoveAt(int index) {
                        list.RemoveAt(index);
                        isDirty = true;
                    }

                    string IList<string>.this[int index] {
                        get {
                            return list[index];
                        }
                        set {
                            list[index] = value;
                            isDirty = true;
                        }
                    }

                    void ICollection<string>.Add(string item) {
                        list.Add(item);
                        isDirty = true;
                    }

                    void ICollection<string>.Clear() {
                        if (list.Count > 0) {
                            list.Clear();
                            isDirty = true;
                        }
                    }

                    bool ICollection<string>.Contains(string item) {
                        return list.Contains(item);
                    }

                    void ICollection<string>.CopyTo(string[] array, int arrayIndex) {
                        list.CopyTo(array, arrayIndex);
                    }

                    int ICollection<string>.Count {
                        get { return list.Count; }
                    }

                    bool ICollection<string>.IsReadOnly {
                        get { return false; }
                    }

                    bool ICollection<string>.Remove(string item) {
                        bool r;
                        if (r = list.Remove(item)) {
                            isDirty = true;
                        }
                        return r;
                    }

                    IEnumerator<string> IEnumerable<string>.GetEnumerator() {
                        return list.GetEnumerator();
                    }

                    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                        return list.GetEnumerator();
                    }
                }
            }
        }
    }
}
