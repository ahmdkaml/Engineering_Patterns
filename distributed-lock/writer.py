import multiprocessing
from lock import Lock


def try_lock():
    lock = Lock()

    if lock.acquire():
        print(f"{multiprocessing.current_process().name} acquired the lock")
    else:
        print(f"{multiprocessing.current_process().name} failed to acquire the lock")


if __name__ == "__main__":
    try:
        import os
        os.remove(".lock")
    except FileNotFoundError:
        pass

    processes = []

    for _ in range(10):
        process = multiprocessing.Process(target=try_lock)
        processes.append(process)
        process.start()

    for process in processes:
        process.join()
