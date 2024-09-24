--
-- PostgreSQL database dump
--

-- Dumped from database version 16.4 (Debian 16.4-1.pgdg120+1)
-- Dumped by pg_dump version 16.4 (Debian 16.4-1.pgdg120+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: CategoryOfEventEntities; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."CategoryOfEventEntities" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL
);


ALTER TABLE public."CategoryOfEventEntities" OWNER TO postgres;

--
-- Name: EventEntities; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."EventEntities" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL,
    "Description" text NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "LocationId" uuid NOT NULL,
    "CategoryId" uuid NOT NULL,
    "MaxNumberOfMembers" integer NOT NULL,
    "ImageUrl" text NOT NULL
);


ALTER TABLE public."EventEntities" OWNER TO postgres;

--
-- Name: LocationsOfEventEntities; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."LocationsOfEventEntities" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL
);


ALTER TABLE public."LocationsOfEventEntities" OWNER TO postgres;

--
-- Name: MemberOfEventEntities; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."MemberOfEventEntities" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "LastName" text NOT NULL,
    "Birthday" timestamp with time zone NOT NULL,
    "DateOfRegistration" timestamp with time zone NOT NULL,
    "Email" text NOT NULL,
    "UserId" uuid NOT NULL,
    "EventId" uuid NOT NULL
);


ALTER TABLE public."MemberOfEventEntities" OWNER TO postgres;

--
-- Name: RefreshTokenEntities; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."RefreshTokenEntities" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Token" text NOT NULL,
    "Expires" timestamp with time zone NOT NULL
);


ALTER TABLE public."RefreshTokenEntities" OWNER TO postgres;

--
-- Name: UserEntities; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserEntities" (
    "Id" uuid NOT NULL,
    "UserName" text NOT NULL,
    "UserEmail" text NOT NULL,
    "Password" text NOT NULL,
    "Role" text NOT NULL
);


ALTER TABLE public."UserEntities" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: CategoryOfEventEntities; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."CategoryOfEventEntities" ("Id", "Title") FROM stdin;
3fa85f64-5717-4562-b3fc-2c963f66afa1	Concerts
3fa85f64-5717-4562-b3fc-2c963f66afa2	Festivals
3fa85f64-5717-4562-b3fc-2c963f66afa3	Sporting Events
3fa85f64-5717-4562-b3fc-2c963f66afa4	Webinars
3fa85f64-5717-4562-b3fc-2c963f66afa5	Workshops
3fa85f64-5717-4562-b3fc-2c963f66afa6	Theaters
3fa85f64-5717-4562-b3fc-2c963f66afa7	Exhibitions
3fa85f64-5717-4562-b3fc-2c963f66afa8	Movie Screenings
3fa85f64-5717-4562-b3fc-2c963f66afa9	Seminars
3fa85f64-5717-4562-b3fc-2c963f66afaa	Conferences
\.


--
-- Data for Name: EventEntities; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."EventEntities" ("Id", "Title", "Description", "Date", "LocationId", "CategoryId", "MaxNumberOfMembers", "ImageUrl") FROM stdin;
3fa85f64-5717-4562-b3fc-2c963f66afa3	Football Match	Local football league match.	2024-09-30 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f13	3fa85f64-5717-4562-b3fc-2c963f66afa3	1000	/images/3fb49744-8468-49cc-bfa6-fb09ab54cae6.webp
3fa85f64-5717-4562-b3fc-2c963f66afa4	Webinar on AI	A webinar discussing the future of AI.	2024-10-05 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f14	3fa85f64-5717-4562-b3fc-2c963f66afa4	200	/images/6c73111e-631c-489f-a7ff-87c5d8c46462.webp
3fa85f64-5717-4562-b3fc-2c963f66afb8	Data Science Webinar	Webinar on the basics of data science.	2024-10-08 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f14	3fa85f64-5717-4562-b3fc-2c963f66afa4	300	/images/565a5fea-8e72-40d4-ba77-508d192169be.webp
3fa85f64-5717-4562-b3fc-2c963f66afa1	Rock Concert	A live rock concert with famous bands.	2024-10-10 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f15	3fa85f64-5717-4562-b3fc-2c963f66afa1	500	/images/39bef33e-9529-4175-89b3-9c2260613902.webp
3fa85f64-5717-4562-b3fc-2c963f66afae	Digital Marketing Webinar	A webinar on effective digital marketing strategies.	2024-10-12 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f14	3fa85f64-5717-4562-b3fc-2c963f66afa4	250	/images/7ea04931-4a1f-4509-9515-37c9920c7c57.webp
3fa85f64-5717-4562-b3fc-2c963f66afac	Food Festival	International food festival with diverse cuisines.	2024-10-15 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f12	3fa85f64-5717-4562-b3fc-2c963f66afa2	400	/images/dec84b7a-2001-4a39-8348-1a93a31985d7.jpg
3fa85f64-5717-4562-b3fc-2c963f66afb9	Baking Workshop	Hands-on workshop on baking desserts.	2024-10-17 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f15	3fa85f64-5717-4562-b3fc-2c963f66afa5	40	/images/b3232643-f975-4f0b-aa40-dfa96772eb41.jpg
3fa85f64-5717-4562-b3fc-2c963f66afa5	Cooking Workshop	Learn to cook traditional dishes.	2024-10-18 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f15	3fa85f64-5717-4562-b3fc-2c963f66afa5	50	/images/f1fe6a3e-dc51-4e16-8324-66548daa3846.jpg
3fa85f64-5717-4562-b3fc-2c963f66afa8	Movie Night	Outdoor movie screening of classic films.	2024-09-25 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f18	3fa85f64-5717-4562-b3fc-2c963f66afa8	200	/images/e41465a0-2324-4ddd-8908-6ebf5d38a2ca.jpg
3fa85f64-5717-4562-b3fc-2c963f66afb2	Classic Film Screening	A special screening of old classic films.	2024-09-27 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f18	3fa85f64-5717-4562-b3fc-2c963f66afa8	150	/images/30bef2e3-2e7e-42f3-b5c5-512890518c71.jpg
3fa85f64-5717-4562-b3fc-2c963f66afab	Jazz Concert	A relaxing evening of jazz music.	2024-09-28 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f15	3fa85f64-5717-4562-b3fc-2c963f66afa1	300	/images/447a856e-9d5c-4bcd-ad3b-f50c241df37d.jpg
3fa85f64-5717-4562-b3fc-2c963f66afb7	Tennis Championship	International tennis championship.	2024-09-29 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f13	3fa85f64-5717-4562-b3fc-2c963f66afa3	1500	/images/a1a051f2-e7bd-42e1-b540-293bd81746cd.png
3fa85f64-5717-4562-b3fc-2c963f66afb6	Cultural Festival	Festival celebrating diverse cultures.	2024-10-20 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f12	3fa85f64-5717-4562-b3fc-2c963f66afa2	500	/images/bc83a33a-60d3-45ab-938c-629f5065d7c1.jpg
3fa85f64-5717-4562-b3fc-2c963f66afaf	Photography Workshop	Learn the art of photography from professionals.	2024-10-22 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f15	3fa85f64-5717-4562-b3fc-2c963f66afa5	60	/images/09cccf3c-d576-4c8a-b979-fb70c69fabcf.webp
3fa85f64-5717-4562-b3fc-2c963f66afb3	Leadership Seminar	A seminar on developing leadership skills.	2024-10-25 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f19	3fa85f64-5717-4562-b3fc-2c963f66afa9	350	/images/9d12725d-7e4e-417f-9059-8e7e05a90103.webp
3fa85f64-5717-4562-b3fc-2c963f66afa6	Theater Play	A famous play performed by local actors.	2024-10-25 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f16	3fa85f64-5717-4562-b3fc-2c963f66afa6	150	/images/dc397e68-76e6-4adf-b644-8701c9bc9f58.jpg
3fa85f64-5717-4562-b3fc-2c963f66afb5	Opera Concert	An opera concert with world-renowned singers.	2024-11-01 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f15	3fa85f64-5717-4562-b3fc-2c963f66afa1	400	/images/2e972cfe-2c08-4b75-a41e-1ee16092a581.jpg
3fa85f64-5717-4562-b3fc-2c963f66afad	Basketball Game	Professional basketball league game.	2024-11-02 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f13	3fa85f64-5717-4562-b3fc-2c963f66afa3	2000	/images/a23875fb-3645-4253-b728-6aabc211cfd2.jpg
3fa85f64-5717-4562-b3fc-2c963f66afbd	Entrepreneurship Seminar	A seminar for aspiring entrepreneurs.	2024-11-03 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f19	3fa85f64-5717-4562-b3fc-2c963f66afa9	400	/images/478c756e-5e4e-46e8-8a2a-c35bbaeffba7.png
3fa85f64-5717-4562-b3fc-2c963f66afa9	Business Seminar	Seminar for business leaders and entrepreneurs.	2024-11-05 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f19	3fa85f64-5717-4562-b3fc-2c963f66afa9	300	/images/7581b0be-f259-49f0-a7b4-04851f197353.jpg
3fa85f64-5717-4562-b3fc-2c963f66afb0	Musical Theater Show	A musical show performed by a local troupe.	2024-11-11 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f16	3fa85f64-5717-4562-b3fc-2c963f66afa6	180	/images/c988c78b-fb9c-47fc-a0d6-785605097173.jpg
3fa85f64-5717-4562-b3fc-2c963f66afba	Drama Play	A dramatic play based on classic literature.	2024-11-14 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f16	3fa85f64-5717-4562-b3fc-2c963f66afa6	200	/images/d26c2409-1e24-440c-9892-db74dc48c7ba.jpg
3fa85f64-5717-4562-b3fc-2c963f66afa2	Film Festival	Annual international film festival.	2024-11-15 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f12	3fa85f64-5717-4562-b3fc-2c963f66afa2	300	/images/9a104efe-6dc0-4d1d-8045-35700f6d5df0.jpg
3fa85f64-5717-4562-b3fc-2c963f66afa7	Art Exhibition	Contemporary art exhibition.	2024-11-20 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f17	3fa85f64-5717-4562-b3fc-2c963f66afa7	100	/images/403fd25d-e90b-4585-be29-1590bd7059d2.jpg
3fa85f64-5717-4562-b3fc-2c963f66afb1	Sculpture Exhibition	A unique sculpture art exhibition.	2024-11-25 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f17	3fa85f64-5717-4562-b3fc-2c963f66afa7	120	/images/ecaeef9b-6351-4c71-b99f-7fd96caa18be.webp
3fa85f64-5717-4562-b3fc-2c963f66afaa	Tech Conference	Conference on the latest tech trends.	2024-12-01 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f1a	3fa85f64-5717-4562-b3fc-2c963f66afaa	500	/images/997e5247-e2c6-4077-ae31-94d5a889828b.jpeg
3fa85f64-5717-4562-b3fc-2c963f66afbe	AI Technology Conference	Conference on artificial intelligence and its applications.	2024-12-02 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f1a	3fa85f64-5717-4562-b3fc-2c963f66afaa	700	/images/1ccdaaaf-e77d-4592-9d91-7794d03628ea.jpg
3fa85f64-5717-4562-b3fc-2c963f66afb4	Blockchain Conference	Conference on blockchain technology and its future.	2024-12-03 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f1a	3fa85f64-5717-4562-b3fc-2c963f66afaa	600	/images/88705d53-d7cb-44c8-aa1c-83616e8a0fd2.jpg
3fa85f64-5717-4562-b3fc-2c963f66afbb	Art Gallery Opening	Opening of a new modern art gallery.	2024-12-10 00:00:00+00	59ba0b28-701a-47c2-9db0-ea309df95f17	3fa85f64-5717-4562-b3fc-2c963f66afa7	80	/images/25b26ccd-e645-4284-a3ba-c2bff97cf0a4.jpg
3fa85f64-5717-4562-b3fc-2c963f66afbc	Outdoor Cinema	A night of open-air cinema.	2024-09-26 00:00:00+00	fed4fe7e-370a-48a9-a16f-51fe780f571b	3fa85f64-5717-4562-b3fc-2c963f66afa8	250	/images/42b23e86-9f45-4bae-a0db-b1914c181c63.webp
\.


--
-- Data for Name: LocationsOfEventEntities; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."LocationsOfEventEntities" ("Id", "Title") FROM stdin;
59ba0b28-701a-47c2-9db0-ea309df95f11	Minsk
59ba0b28-701a-47c2-9db0-ea309df95f12	New York
59ba0b28-701a-47c2-9db0-ea309df95f13	London
59ba0b28-701a-47c2-9db0-ea309df95f14	Tokyo
59ba0b28-701a-47c2-9db0-ea309df95f15	Berlin
59ba0b28-701a-47c2-9db0-ea309df95f16	Paris
59ba0b28-701a-47c2-9db0-ea309df95f17	Sydney
59ba0b28-701a-47c2-9db0-ea309df95f18	Moscow
59ba0b28-701a-47c2-9db0-ea309df95f19	Toronto
59ba0b28-701a-47c2-9db0-ea309df95f1a	Dubai
fed4fe7e-370a-48a9-a16f-51fe780f571b	Slonim
\.


--
-- Data for Name: MemberOfEventEntities; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."MemberOfEventEntities" ("Id", "Name", "LastName", "Birthday", "DateOfRegistration", "Email", "UserId", "EventId") FROM stdin;
\.


--
-- Data for Name: RefreshTokenEntities; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."RefreshTokenEntities" ("Id", "UserId", "Token", "Expires") FROM stdin;
7d74434b-0be9-448e-9c20-cdcfbc7b6499	b3f1af10-8b3c-4c78-8381-f4f4caf1b74f	Qe667g+8kxRTPP1774QCVhee/GrN0QzcvdmSrAiuAMRAkZzMZbOMNZB1bq9mqkXzEV8PumPeaZvZJaDehIJ0Tw==	2024-09-30 19:21:14.088956+00
\.


--
-- Data for Name: UserEntities; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserEntities" ("Id", "UserName", "UserEmail", "Password", "Role") FROM stdin;
b3f1af10-8b3c-4c78-8381-f4f4caf1b74f	admin	admin@gmail.com	$2a$11$mwc66uMZljdgUCeH07GYNexvMzBEDKYuiecXpN4nPG5tKXptlrXQO	Admin
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20240923191514_init	8.0.8
\.


--
-- Name: CategoryOfEventEntities PK_CategoryOfEventEntities; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."CategoryOfEventEntities"
    ADD CONSTRAINT "PK_CategoryOfEventEntities" PRIMARY KEY ("Id");


--
-- Name: EventEntities PK_EventEntities; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."EventEntities"
    ADD CONSTRAINT "PK_EventEntities" PRIMARY KEY ("Id");


--
-- Name: LocationsOfEventEntities PK_LocationsOfEventEntities; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LocationsOfEventEntities"
    ADD CONSTRAINT "PK_LocationsOfEventEntities" PRIMARY KEY ("Id");


--
-- Name: MemberOfEventEntities PK_MemberOfEventEntities; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MemberOfEventEntities"
    ADD CONSTRAINT "PK_MemberOfEventEntities" PRIMARY KEY ("Id");


--
-- Name: RefreshTokenEntities PK_RefreshTokenEntities; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."RefreshTokenEntities"
    ADD CONSTRAINT "PK_RefreshTokenEntities" PRIMARY KEY ("Id");


--
-- Name: UserEntities PK_UserEntities; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserEntities"
    ADD CONSTRAINT "PK_UserEntities" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_CategoryOfEventEntities_Title; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_CategoryOfEventEntities_Title" ON public."CategoryOfEventEntities" USING btree ("Title");


--
-- Name: IX_EventEntities_CategoryId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_EventEntities_CategoryId" ON public."EventEntities" USING btree ("CategoryId");


--
-- Name: IX_EventEntities_LocationId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_EventEntities_LocationId" ON public."EventEntities" USING btree ("LocationId");


--
-- Name: IX_MemberOfEventEntities_EventId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_MemberOfEventEntities_EventId" ON public."MemberOfEventEntities" USING btree ("EventId");


--
-- Name: IX_MemberOfEventEntities_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_MemberOfEventEntities_UserId" ON public."MemberOfEventEntities" USING btree ("UserId");


--
-- Name: IX_RefreshTokenEntities_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_RefreshTokenEntities_UserId" ON public."RefreshTokenEntities" USING btree ("UserId");


--
-- Name: IX_UserEntities_UserEmail; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_UserEntities_UserEmail" ON public."UserEntities" USING btree ("UserEmail");


--
-- Name: IX_UserEntities_UserName; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_UserEntities_UserName" ON public."UserEntities" USING btree ("UserName");


--
-- Name: EventEntities FK_EventEntities_CategoryOfEventEntities_CategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."EventEntities"
    ADD CONSTRAINT "FK_EventEntities_CategoryOfEventEntities_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES public."CategoryOfEventEntities"("Id") ON DELETE RESTRICT;


--
-- Name: EventEntities FK_EventEntities_LocationsOfEventEntities_LocationId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."EventEntities"
    ADD CONSTRAINT "FK_EventEntities_LocationsOfEventEntities_LocationId" FOREIGN KEY ("LocationId") REFERENCES public."LocationsOfEventEntities"("Id") ON DELETE RESTRICT;


--
-- Name: MemberOfEventEntities FK_MemberOfEventEntities_EventEntities_EventId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MemberOfEventEntities"
    ADD CONSTRAINT "FK_MemberOfEventEntities_EventEntities_EventId" FOREIGN KEY ("EventId") REFERENCES public."EventEntities"("Id") ON DELETE CASCADE;


--
-- Name: MemberOfEventEntities FK_MemberOfEventEntities_UserEntities_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MemberOfEventEntities"
    ADD CONSTRAINT "FK_MemberOfEventEntities_UserEntities_UserId" FOREIGN KEY ("UserId") REFERENCES public."UserEntities"("Id") ON DELETE CASCADE;


--
-- Name: RefreshTokenEntities FK_RefreshTokenEntities_UserEntities_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."RefreshTokenEntities"
    ADD CONSTRAINT "FK_RefreshTokenEntities_UserEntities_UserId" FOREIGN KEY ("UserId") REFERENCES public."UserEntities"("Id") ON DELETE RESTRICT;


--
-- PostgreSQL database dump complete
--

