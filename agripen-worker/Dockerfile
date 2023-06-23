FROM tensorflow/tensorflow:2.10.0

ARG MODEL_URL
ARG CLASS_NAMES_URL

WORKDIR /workdir

RUN mkdir -p "/workdir/data"
ADD $MODEL_URL /workdir/data/model.h5
ADD $CLASS_NAMES_URL /workdir/data/class_names.z

RUN apt-get update \
  && apt-get install -y --no-install-recommends ca-certificates curl gnupg gpg \
  && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
  && curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list \
  && apt-get update \
  && ACCEPT_EULA=Y apt-get install -y msodbcsql17 mssql-tools gcc g++ \
  && apt-get install -y unixodbc unixodbc-dev \
  && apt-get clean \
  && rm -rf /var/lib/apt/lists/* \
  && echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bash_profile \
  && echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc

COPY ./requirements.txt /workdir/requirements.txt
RUN pip install --no-cache-dir -r requirements.txt

COPY ./app /workdir/app

CMD ["python3", "/workdir/app/worker.py"]
