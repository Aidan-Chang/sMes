namespace DataArchive.Model;

public static class Options {

    public static int[] BatchSizes = { 50, 100, 500, 1000 };

    public enum RecoveryMode {
        Simple,
        Full,
        NotSet,
    }

    public enum Mode {
        Copy,
        Move,
    }

}