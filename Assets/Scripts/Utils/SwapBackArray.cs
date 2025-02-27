using System;
using System.Collections.Generic;

/// <summary>
/// It's a List&lt;T&gt; but Remove/RemoveAt/RemoveAll are EXTREMELY fast by not guaranteeing item order is maintained
/// </summary>
public class SwapBackArray<T> : List<T> {
    public new bool Remove(T item) {
        var i = IndexOf(item);
        if (i == -1) {
            return false;
        }

        this[i] = this[^1];
        base.RemoveAt(Count - 1);

        return true;
    }

    public new void RemoveAt(int index) {
        this[index] = this[^1];
        base.RemoveAt(Count - 1);
    }

    public new void RemoveAll(Predicate<T> match) {
        var removed = 0;

        for (var i = 0; i < Count - removed; i++) {
            if (!match(this[i])) {
                continue;
            }

            this[i] = this[^(1 + removed)];
            removed++;
            i--;
        }

        if (removed > 0) {
            base.RemoveRange(Count - removed, removed);
        }
    }
}