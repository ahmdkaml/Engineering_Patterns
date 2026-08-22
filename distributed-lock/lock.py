class Lock:
    def acquire(self):
        try:
            open(".lock", "x").close()
            return True
        except FileExistsError:
            return False


