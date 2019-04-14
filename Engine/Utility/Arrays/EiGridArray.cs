using System;

namespace Eitrum {
    public interface EiGridClearInterface {
        void OnClear();
    }

    public class EiGridArray<T> {
        #region Variables

        int width;
        int height;

        T[] array;

        #endregion

        #region Properties

        public T this[int x, int y] {
            get {
                return array[x * height + y];
            }
            set {
                array[x * height + y] = value;
            }
        }

        public int Width {
            get {
                return width;
            }
        }

        public int Height {
            get {
                return height;
            }
        }

        public int Length {
            get {
                return width * height;
            }
        }

        #endregion

        #region Constructor

        public EiGridArray(int width, int height) {
            if (width < 1 || height < 1)
                throw new ArgumentOutOfRangeException("Width or Height is to small, has to be 1 or above");
            this.width = width;
            this.height = height;
            array = new T[width * height];
        }

        #endregion

        #region Set

        public void SetValue(int index, T value) {
            array[index] = value;
        }

        public void SetValue(int x, int y, T value) {
            array[x * height + y] = value;
        }

        public void SetRow(int y, T[] row) {
            for (int i = 0; i < width; i++) {
                array[i * height + y] = row[i];
            }
        }

        public void SetColumn(int x, T[] col) {
            for (int i = 0; i < height; i++) {
                array[x * height + i] = col[i];
            }
        }

        #endregion

        #region Get

        public T GetValue(int index) {
            return array[index];
        }

        public T GetValue(int x, int y) {
            return array[x * height + y];
        }

        public T[] GetRow(int y) {
            T[] temp = new T[width];
            for (int i = 0; i < width; i++) {
                temp[i] = array[i * height + y];
            }
            return temp;
        }

        public T[] GetColumn(int x) {
            T[] temp = new T[height];
            for (int i = 0; i < height; i++) {
                temp[i] = array[x * height + i];
            }
            return temp;
        }

        #endregion

        #region Helper

        public void Clear() {
            for (int w = 0; w < width; w++) {
                for (int h = 0; h < height; h++) {
                    EiGridClearInterface clearInterface = this[w, h] as EiGridClearInterface;
                    if (clearInterface != null)
                        clearInterface.OnClear();
                    this[w, h] = default(T);
                }
            }
        }

        public void SetDefaultValues() {
            Clear();
        }

        #endregion
    }
}

